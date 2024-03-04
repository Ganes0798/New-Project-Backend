using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using New_Project_Backend.Model;
using Project.Core.CustomModels;
using Project.Core.Data;
using Project.Core.Extensions;
using Project.Core.Interface;
using static Project.Core.Enums.CommonEnums;
using static Project.Core.Model.CutomResults;

namespace New_Project_Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : BaseController
	{
        public UserController(IConfiguration configuration): base(configuration)
        {
            
        }


        [HttpGet]
        public ActionResult Get([FromQuery] long? id)
        {
            var UserDetails = GetCurrentUserDetail();
            Roles _userRole = Roles.None;
			Enum.TryParse(UserDetails.RoleName.ToString(), true, out _userRole);

			List<Roles> filterRole = new List<Roles>() { Roles.User };
			using (ExtendedProjectDbContext dbcontext = new ExtendedProjectDbContext(c_config))
            {
                var _userDetails = dbcontext.Users.Where(xy => ((id.HasValue ? (xy.Id == id) : true) && (filterRole.Contains(xy.RoleName)) &&
                                                                 (xy.DataState == RecordState.Active)))
                                                                 .OrderBy(xy => xy.Id)
                                                                 .Select(xy => new UserR()
                                                                 {
                                                                     Id = xy.Id,
                                                                     FirstName = xy.FirstName,
                                                                     LastName = xy.LastName,
                                                                     Email = xy.Email,
                                                                     UserRole = xy.RoleName,
                                                                     ModifiedOn = xy.ModifiedOn
                                                                 }).ToList();

				if ((_userDetails == null) || (_userDetails.Count == 0))
				{
					return SendErrorMessage(ErrorCodes.EmptyUsers);
				}
				else
				{
					if (id.HasValue)
					{

						return Ok(new ResponseBodyResource<UserR>()
						{
							Result = _userDetails[0]
						});
					}
					else
					{
						return Ok(new ResponseBodyResource<List<UserR>>()
						{
							Result = _userDetails
						});
					}
				}

			}
        }

		[HttpPatch("Password")]
		public ActionResult ChangePassword([FromBody] chngePwd _request)
		{
			var _userDetails = GetCurrentUserDetail();
			try
			{
				using (ExtendedProjectDbContext dbContext = new ExtendedProjectDbContext(c_config))
				{
					var _existUser = GetUserDetails(dbContext, _userDetails.Id);
					if (_existUser == null)
					{
						return SendErrorMessage(ErrorCodes.EmptyUsers);
					}

					string encPwd = EncryptDecrypt.EncryptString(_request.OldPassword);
					if (_existUser.Password != encPwd)
					{
						return SendErrorMessage(ErrorCodes.PasswordMismatch);
					}

					_existUser.Password = EncryptDecrypt.EncryptString(_request.NewPassword);
					_existUser.ModifiedOn = DateTime.UtcNow;

					using (var transaction = dbContext.Database.BeginTransaction())
					{
						try
						{
							dbContext.Entry(_existUser).State = EntityState.Modified;
							dbContext.SaveChanges();
							transaction.Commit();
							_existUser.Password = string.Empty;
						}
						catch (Exception)
						{
							transaction.Rollback();
							throw;
						}
					}

					return SendSuccessMessage(ErrorCodes.PasswordUpdatedSuccessfully);
					//return Ok(new ResponseBodyResource<Users>()
					//{
					//    Result = _existUser
					//});
				}

			}
			catch (Exception)
			{
				throw;
			}
		}



	}
}
