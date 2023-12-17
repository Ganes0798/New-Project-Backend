using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

		//<param name="Id">Id of the user.</param>
		[HttpGet]
		public IActionResult Get([FromQuery] int? Id)
		{
			using (ExtendedProjectDbContext dbContext = new ExtendedProjectDbContext(Configuration))
			{
				var _userList = dbContext.Registration.Where(xy => (Id.HasValue ? (xy.Id == Id) : true && (xy.DataState == RecordState.Active))).OrderBy(xy => xy.Id)
								  .Select(ab => new Register()
								  {
									  Id = ab.Id,
									  Username = ab.Username,
									  Email = ab.Email,
									  RoleName = ab.RoleName,
									  termAccept = ab.termAccept,
									  CreatedOn = ab.CreatedOn,
									  ModifiedOn = ab.ModifiedOn
								  }).ToList();

				//if((_userList == null) || (_userList.Count == 0))
				//{
				//	return BadRequest(ErrorCodes.EmptyUserList.ToString());
				//}
				if (Id.HasValue)
				{

					return Ok(new ResponseBodyResource<Register>()
					{
						Result = _userList[0]
					});
				}
				else
				{

					return Ok(new ResponseBodyResource<List<Register>>()
					{
						Result = _userList
					});
				}
				//return Ok(new ResponseBodyResource<Register>()
				//{
				//	Result = _userList[0]
				//});
			}
		}


		[HttpPost]
		public IActionResult Add([FromBody] Register signin)
		{

			if (signin != null)
			{
				using (ExtendedProjectDbContext dbcontext = new ExtendedProjectDbContext(Configuration))
				{
					if (IsEmailExists(dbcontext, signin.Email, 0))
					{
						return BadRequest(ErrorCodes.EmailAlreadyExists.ToString());
					}
					var _newUser = new Register()
					{
						Id = signin.Id,
						Username = signin.Username,
						Email = signin.Email,
						Password = Extensions.EncryptDecrypt.EncryptString(signin.Password),
						RoleName = signin.RoleName,
						termAccept = signin.termAccept

					};

					dbcontext.Registration.Add(_newUser);
					dbcontext.SaveChanges();
				}


				return Ok(new ResponseBodyResource<Register>()
				{
					Message = ErrorCodes.NewUserAddedSuccessFully.ToString(),
					Result = signin
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
