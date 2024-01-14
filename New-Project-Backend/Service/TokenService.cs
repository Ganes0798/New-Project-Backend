﻿using Microsoft.IdentityModel.Tokens;
using Project.Core.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace New_Project_Backend.Service
{
	public class TokenService : ITokenService
	{
		private readonly IConfiguration _config;
		private readonly SymmetricSecurityKey _key;
		public TokenService(IConfiguration config)
		{
			_config = config;
			_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]!));
		}

		public string CreateToken(UserDetails userDetails)
		{
			var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new List<Claim>
				{
					new Claim(ClaimTypes.Email, userDetails.Email),
					new Claim(ClaimTypes.NameIdentifier, userDetails.Id.ToString()),
					new Claim(ClaimTypes.Role, userDetails.RoleName.ToString()),
					new Claim(ClaimTypes.Name, userDetails.UserName),
				}),
				Expires = DateTime.UtcNow.AddDays(10),
				SigningCredentials = creds,
				Issuer = _config["Token:Issuer"]
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
		}
	}
}
