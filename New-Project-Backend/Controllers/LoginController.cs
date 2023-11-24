using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using New_Project_Backend.Data;
using New_Project_Backend.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
		public IActionResult register(Login login)
		{
			_db.Add(login);
			_db.SaveChanges();

			return Ok(login);
		}

		[HttpPost("Login")]
		public async Task<IActionResult> login(Signin signin)
		{

			if (signin != null)
			{
				var loginCheck = _db.Registration.Where(e => e.Email == signin.Email && e.Password == signin.Password).FirstOrDefault();
				if (loginCheck == null)
				{
					return BadRequest("Inavlid Credentials");
				}
				else
				{
					//signin.AccessToken = GetToken(signin);
					_db.Add(signin);
					_db.SaveChanges();
					return Ok(signin);
				}
			}
			else
			{
				return BadRequest("No Data Posted");
			};


		}

		//public string GetToken(Signin signin)
		//{
		//	var claims = new[]
		//	{
		//		new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]),
		//		new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
		//		new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
		//		new Claim("Id", signin.id.ToString()),
		//		new Claim("Email", signin.Email),
		//	};

		//	var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
		//	var signedin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
		//	var token = new JwtSecurityToken(
		//		_config["Jwt:Issuer"],
		//		_config["Jwt:Audience"],
		//		claims,
		//		expires: DateTime.UtcNow.AddDays(10),
		//		signingCredentials: signedin);

		//	string Token = new JwtSecurityTokenHandler().WriteToken(token);

		//	return Token;
		//}
    }
}
