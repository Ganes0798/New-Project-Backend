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
using Project.Core.Interface;
using static Project.Core.Enums.CommonEnums;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Cors;


namespace New_Project_Backend.Controllers
{
	[EnableCors("_allowOriginPolicy")]
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		private readonly IConfiguration _config;
		private readonly ITokenService c_tokenService;
		private readonly ApplicationSettings _googleAuthSettings;
		private static List<UserDetails> UserList = new List<UserDetails>();
		//public ITokenService tokenService;


		public LoginController(IConfiguration configuration, ITokenService token, IOptions<ApplicationSettings> googleAuthSettings)
		{
			_config = configuration;
			c_tokenService = token;
			_googleAuthSettings = googleAuthSettings.Value;

		}



		[HttpPost]
		public IActionResult Add([FromBody] Login signin)
		{
			
			if (signin != null)
			{
				using (ExtendedProjectDbContext dbcontext = new ExtendedProjectDbContext(_config))
				{
					var _userProfile = dbcontext.Users.Where(xy => (xy.Email == signin.Email))
											.Select(xy => new DetailToken()
											{
												Id = xy.Id,
												Email = xy.Email,
												Password = xy.Password,
												UserName = xy.FirstName + xy.LastName,
												RoleName = xy.RoleName.ToString(),
											}).FirstOrDefault();


					if (_userProfile == null)
					{
						return new ObjectResult(new ResponseBodyResource<ErrorCodes>(ErrorCodes.EmailDoesNotExist));
					}


					_userProfile.Password = string.Empty;
					_userProfile.Token = c_tokenService.CreateToken(_userProfile);



					if(_userProfile != null)
					{
						return Ok(new ResponseBodyResource<DetailToken>()
						{
							Message = ErrorCodes.LoggedInSuccessFully.ToString(),
							Result = _userProfile
						});
					}
					else
					{
						return Ok(new ResponseBodyResource<Login>()
						{
							Message = ErrorCodes.UnableToLogin.ToString(),
						});

					}
				}
				


			}
			else
			{
				return BadRequest(ErrorCodes.UnableToLogin.ToString());
			};


		}

		[HttpPost("LoginwithGoogle")]
		public async Task<IActionResult> AddGoogle([FromBody] string credential)
		{
			try
			{
				var settings = new GoogleJsonWebSignature.ValidationSettings
				{
					Audience = new List<string> { _googleAuthSettings.GoogleClientId },
				};

				var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);

				using (ExtendedProjectDbContext dbcontext = new ExtendedProjectDbContext(_config))
				{
					var _userProfile = dbcontext.Users.Where(xy => (xy.Email == 
					payload.Email))
											.Select(xy => new DetailToken()
											{
												Id = xy.Id,
												Email = xy.Email,
												Password = xy.Password,
												UserName = xy.FirstName + xy.LastName,
												RoleName = xy.RoleName.ToString(),
											}).FirstOrDefault();


					if (_userProfile == null)
					{
						return BadRequest("Email DoesNot Exists");
					}


					_userProfile.Password = string.Empty;
					_userProfile.Token = c_tokenService.CreateToken(_userProfile);



					if (_userProfile != null)
					{
						return Ok(new ResponseBodyResource<DetailToken>()
						{
							Message = ErrorCodes.LoggedInSuccessFully.ToString(),
							Result = _userProfile
						});
					}
					else
					{
						return Ok(new ResponseBodyResource<DetailToken>()
						{
							Message = ErrorCodes.UnableToLogin.ToString(),
						});

					}
				}
			}
			catch (InvalidJwtException ex)
			{
				// Handle invalid JWT exceptions here
				return BadRequest("Invalid JWT: " + ex.Message);
			}
			catch (Exception ex)
			{
				// Handle other exceptions here
				return StatusCode(500, "Internal server error: " + ex.Message);
			}
		}

		[HttpPost("GoogleSignUp")]	
		public async Task<IActionResult> SignUpGoogle([FromBody] string credential)
		{
			try
			{
				var settings = new GoogleJsonWebSignature.ValidationSettings
				{
					Audience = new List<string> { _googleAuthSettings.GoogleClientId },
				};

				var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);

				using (ExtendedProjectDbContext dbContext = new ExtendedProjectDbContext(_config))
				{
					if (IsEmailExists(dbContext, payload.Email, 0))
					{
						return BadRequest(ErrorCodes.UserAlreadyExist.ToString());
					}

					Roles _userRole = Roles.User;
					//Enum.TryParse(register.RoleName.ToString(), true, out _userRole);
					//if (!Enum.IsDefined(typeof(Roles), _userRole))
					//{
					//	return SendErrorMessage(ErrorCodes.InvalidEnumRole);
					//}

					var _newUser = new Register()
					{
						FirstName = payload.GivenName,
						LastName = payload.FamilyName,
						Email = payload.Email,
						RoleName = _userRole
					};

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

					//return SendSuccessMessage(ErrorCodes.NewUserAddedSuccessFully);
					return Ok(new ResponseBodyResource<Register>()
					{
						Message = ErrorCodes.NewUserAddedSuccessFully.ToString(),
						Result = _newUser
					});
				}
			}
			catch (InvalidJwtException ex)
			{
				// Handle invalid JWT exceptions here
				return BadRequest("Invalid JWT: " + ex.Message);
			}
			catch (Exception ex)
			{
				// Handle other exceptions here
				return StatusCode(500, "Internal server error: " + ex.Message);
			}
		}



		[HttpPost("logoff")]
		//[AllowAuthoize(new[] { RoleType.SuperAdmin, RoleType.Admin, RoleType.User }, PrivilegeType.DoNotCheck)]
		public IActionResult LogOff()
		{
			var claimsPrincipal = User.Identity;
			string _userName = User.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).FirstOrDefault();
			string _email = User.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault();
			string _Id = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();
			try
			{
				using (ExtendedProjectDbContext dbContext = new ExtendedProjectDbContext(_config))
				{


					return Ok(new ResponseBodyResource<ErrorCodes>()
					{
						ResCode = ErrorCodes.LoggedOffSuccessfully.ToString(),
						Result = ErrorCodes.LoggedOffSuccessfully
					});
				}
			}
			catch (Exception)
			{
				return new ObjectResult(new ResponseBodyResource<ErrorCodes>(ErrorCodes.UnableToLogOff));
			}
		}
		private bool IsEmailExists(ProjectDbContext db, string email, int id)
		{
			return db.Users.Where(xy => (xy.Id != id) && (xy.Email == email)).Any();
		}
	}
}
