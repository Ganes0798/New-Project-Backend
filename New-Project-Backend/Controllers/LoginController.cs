using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using New_Project_Backend.Model;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using Project.Core.CustomModels;
using Project.Core.Data;


namespace New_Project_Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		private readonly ProjectDbContext _db;
		private readonly IConfiguration _config;


        public LoginController(ProjectDbContext db, IConfiguration configuration)
		{
			_db = db;
			_config = configuration;
		}

		[HttpPost("Register")]
		public IActionResult register([FromBody] Login login)
		{
			if(IsEmailExists(_db, login.Email, 0))
			{
                return BadRequest(ErrorCodes.EmailAlreadyExists.ToString());
            }
			if (login != null)
			{
				var _newUser = new Login()
				{
					User_id = login.User_id,
					Username = login.Username,
					Email = login.Email,
					Password = Extensions.EncryptDecrypt.EncryptString(login.Password),
					RoleName = login.RoleName,
					termAccept = login.termAccept

				};
                    _db.Add(_newUser);
                    _db.SaveChanges();
              
			
				return Ok(new ResponseBodyResource<Login>()
				{
					Message = ErrorCodes.NewUserAddedSuccessFully.ToString(),
					Result = login
				});
			}
			else
			{
				return BadRequest(ErrorCodes.UnableToAddUser.ToString());
			}

		}

		[HttpPost("Login")]
		public IActionResult login([FromBody] Signin signin)
		{
			var _loggedin = new Signin()
			{
				Email = signin.Email,
				Password = Extensions.EncryptDecrypt.EncryptString(signin.Password),

			};
			if (signin != null)
			{
                var loginCheck = _db.Registration.Where(e => e.Email == _loggedin.Email && e.Password == _loggedin.Password).FirstOrDefault();
                if (loginCheck != null)
                {

                    return Ok(new ResponseBodyResource<Signin>()
                    {
                        Message = ErrorCodes.LoggedInSuccessFully.ToString(),
                        Result = _loggedin
                    });
                   
				}
				else
				{
                    return Ok(new ResponseBodyResource<Signin>()
                    {
                        Message = ErrorCodes.UnableToLogin.ToString(),
                    });

                }


			}
			else
			{
				return BadRequest(ErrorCodes.UnableToLogin.ToString());
			};


		}

		private bool IsEmailExists(ProjectDbContext db, string email, int id)
		{
			return db.Registration.Where(xy=> (xy.User_id != id) && (xy.Email == email)).Any();
		}
	}

}
