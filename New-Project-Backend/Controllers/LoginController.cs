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
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using New_Project_Backend.Services;

namespace New_Project_Backend.Controllers
{
	[Route("api/[controller]")]
	[Authorize]
	[ApiController]
	public class LoginController : ControllerBase
	{
		private readonly ApplicationDbContext _db;
		private readonly IConfiguration _config;
        private readonly ITokenGeneration c_tokenService;


        public LoginController(ApplicationDbContext db, IConfiguration configuration, ITokenGeneration token)
		{
			_db = db;
			_config = configuration;
			c_tokenService = token;
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
                if (loginCheck != null)
                {
                    var _userProfile = _db.Registration.Where(xy => (xy.Email == signin.Email))
                                             .Select(xy => new Response()
                                             {
                                                 Email = xy.Email,
                                                 Password = xy.Password,
                                                 Username = xy.Username,
                                                 RoleName = xy.RoleName.ToString(),
                                             }).FirstOrDefault();

				       _userProfile.Token = c_tokenService.CreateToken(_userProfile);

                    return Ok(new ResponseBodyResource<Response>()
                    {
                        Message = ErrorCodes.LoggedInSuccessFully.ToString(),
                        Result = _userProfile
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
	}
}
