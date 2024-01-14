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
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Project.Core.Interface;
using static Project.Core.Enums.CommonEnums;
using New_Project_Backend.Extensions;


namespace New_Project_Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		private readonly IConfiguration _config;
		private readonly ITokenService c_tokenService;
		//public ITokenService tokenService;


		public LoginController(IConfiguration configuration, ITokenService token)
		{
			_config = configuration;
			c_tokenService = token;

		}



		[HttpPost]
		public IActionResult Add([FromBody] Login signin)
		{
			
			if (signin != null)
			{
				using (ExtendedProjectDbContext dbcontext = new ExtendedProjectDbContext(_config))
				{
					var _userProfile = dbcontext.Registration.Where(xy => (xy.Email == signin.Email))
											.Select(xy => new DetailToken()
											{
												Id = xy.Id,
												Email = xy.Email,
												Password = xy.Password,
												UserName = xy.Username,
												RoleName = xy.RoleName.ToString(),
											}).FirstOrDefault();


					if (_userProfile == null)
					{
						return new ObjectResult(new ResponseBodyResource<ErrorCodes>(ErrorCodes.EmailDoesNotExist));
					}


					_userProfile.Password = string.Empty;
					_userProfile.Token = c_tokenService.CreateToken(_userProfile);





					return Ok(new ResponseBodyResource<DetailToken>()
					{
						Message = ErrorCodes.LoggedInSuccessFully.ToString(),
						Result = _userProfile
					});

					//else
					//{
					//	return Ok(new ResponseBodyResource<Login>()
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
