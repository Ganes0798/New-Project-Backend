using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using New_Project_Backend.Extensions;
using New_Project_Backend.Model;
using Project.Core.CustomModels;
using Project.Core.Data;
using Project.Core.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Project.Core.Enums.CommonEnums;

namespace New_Project_Backend.Controllers
{
	[EnableCors("_allowOriginPolicy")]
	[Route("api/[controller]")]
	[ApiController]
	public class RegisterController : BaseController
	{

		public RegisterController(IConfiguration config) : base(config)
		{
		}

		[AllowAnonymous]
		[HttpPost]
		public IActionResult Add([FromBody] Register register)
		{

			var _userDetails = GetCurrentUserDetail();
			try
			{
				using (ExtendedProjectDbContext dbContext = new ExtendedProjectDbContext(c_config))
				{
					if (IsEmailExists(dbContext, register.Email, 0))
					{
						return SendErrorMessage(ErrorCodes.UserAlreadyExist);
					}

					Roles _userRole = Roles.User;
					//if (!Enum.IsDefined(typeof(Roles), _userRole))
					//{
					//	return SendErrorMessage(ErrorCodes.InvalidEnumRole);
					//}

					var _newUser = new Register()
					{
						FirstName = register.FirstName,
						LastName = register.LastName,
						Email = register.Email,
						Password = EncryptDecrypt.EncryptString(register.Password),
						ConfirmPassword = EncryptDecrypt.EncryptString(register.ConfirmPassword),
						RoleName = _userRole,
					};
					if(register.ConfirmPassword == register.Password)
					{
						using (var transaction = dbContext.Database.BeginTransaction())
						{
							try
							{
								dbContext.Users.Add(_newUser);
								dbContext.SaveChanges();
								transaction.Commit();
							}
							catch (Exception)
							{
								transaction.Rollback();
								throw;
							}
						}

						return SendSuccessMessage(ErrorCodes.NewUserAddedSuccessFully);
					}
					else
					{
						return SendErrorMessage(ErrorCodes.CheckThePasswords);
					}
					
					//return Ok(new ResponseBodyResource<Users>()
					//{
					//    Result = _newUser
					//});
				}
			}
			catch (Exception)
			{
				throw;
			}

		}

		private bool IsEmailExists(ProjectDbContext db, string email, int id)
		{
			return db.Users.Where(xy => (xy.Id != id) && (xy.Email == email)).Any();
		}
	}
}
