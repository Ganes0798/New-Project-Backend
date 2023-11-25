using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using New_Project_Backend.Data;
using New_Project_Backend.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using New_Project_Backend.Extensions;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Http.HttpResults;

namespace New_Project_Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		private readonly ApplicationDbContext _db;
		private readonly IConfiguration _config;


        public LoginController(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
			_config = configuration;
        }

		[HttpPost("Register")]
		public IActionResult register([FromBody] Login login)
		{
			if (login != null)
			{
				var _newUser = new Login()
				{
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
		public async Task<IActionResult> login([FromBody] Signin signin)
		{
			var _loggedin = new Signin()
			{
				Email = signin.Email,
				Password = Extensions.EncryptDecrypt.EncryptString(signin.Password),
                
            };
            if (signin != null)
            {
				var loginCheck = _db.Registration.Where(e => e.Email == _loggedin.Email && e.Password == _loggedin.Password).FirstOrDefault();
				if (loginCheck == null)
				{
                    return Ok(new ResponseBodyResource<Signin>()
                    {
                        Message = ErrorCodes.UnableToLogin.ToString(),
                    });
                }
				else
				{
                    return Ok(new ResponseBodyResource<Signin>()
                    {
						Message = ErrorCodes.LoggedInSuccessFully.ToString(),
                        Result = signin
                    });
                    
				}

               
            }
			else
			{
				return BadRequest(ErrorCodes.UnableToLogin.ToString());
			};


		}
    }
}
