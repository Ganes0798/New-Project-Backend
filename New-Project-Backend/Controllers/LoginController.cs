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
		private readonly IConfiguration _config;
		//private readonly ITokenService tokenService;


        public LoginController(IConfiguration configuration)
		{
			_config = configuration;
		}

		

		[HttpPost]
		public IActionResult Add([FromBody] Login signin)
		{
			
			if (signin != null)
			{
				using (ExtendedProjectDbContext dbcontext = new ExtendedProjectDbContext(_config))
				{
					//var _userProfile = dbcontext.Registration.Where(xy => (xy.Email == signin.Email)).Select(xy => new ProfileR()
					//{
					//	Id = xy.User_id,
					//	Username = xy.Username,
					//	RoleName = xy.RoleName.ToString(),
					//	Password = xy.Password,
					//}).FirstOrDefault();
					var _loggedin = new Login()
					{
						Email = signin.Email,
						Password = signin.Password
					};
					var loginCheck = dbcontext.Registration.Where(e => e.Email == _loggedin.Email && e.Password == _loggedin.Password).FirstOrDefault();
					_loggedin.Password = string.Empty;
					//_userProfile.Token = tokenService.CreateToken(_userProfile);
					


					return Ok(new ResponseBodyResource<Login>()
						{
							Message = ErrorCodes.LoggedInSuccessFully.ToString(),
							Result = _loggedin
						});

					//else
					//{
					//	return Ok(new ResponseBodyResource<Signin>()
					//	{
					//		Message = ErrorCodes.UnableToLogin.ToString(),
					//	});

					//}
				}
				


			}
			else
			{
				return BadRequest(ErrorCodes.UnableToLogin.ToString());
			};


		}
		
		
	}

}
