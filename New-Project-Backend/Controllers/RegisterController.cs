using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using New_Project_Backend.Extensions;
using New_Project_Backend.Model;
using Project.Core.CustomModels;
using Project.Core.Data;
using static Project.Core.Enums.CommonEnums;

namespace New_Project_Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RegisterController : ControllerBase
	{
		private readonly IConfiguration Configuration;

		public RegisterController(IConfiguration configuration)
		{
			Configuration = configuration;
		}
		
		[HttpPost]
		public IActionResult Add([FromBody] Register register)
		{

			if (register != null)
			{
				using (ExtendedProjectDbContext dbcontext = new ExtendedProjectDbContext(Configuration))
				{
					if (IsEmailExists(dbcontext, register.Email, 0))
					{
						return BadRequest(ErrorCodes.EmailAlreadyExists.ToString());
					}
					var _newUser = new Register()
					{
						Id = register.Id,
						Username = register.Username,
						Email = register.Email,
						Password = EncryptDecrypt.EncryptString(register.Password),
						RoleName = register.RoleName,
						termAccept = register.termAccept

					};

					dbcontext.Registration.Add(_newUser);
					dbcontext.SaveChanges();
				}


				return Ok(new ResponseBodyResource<Register>()
				{
					Message = ErrorCodes.NewUserAddedSuccessFully.ToString(),
					Result = register
				});
			}
			else
			{
				return BadRequest(ErrorCodes.UnableToAddUser.ToString());
			}

		}

		private bool IsEmailExists(ProjectDbContext db, string email, int id)
		{
			return db.Registration.Where(xy => (xy.Id != id) && (xy.Email == email)).Any();
		}
	}
}
