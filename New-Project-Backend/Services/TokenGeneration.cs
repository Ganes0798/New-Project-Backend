using Microsoft.IdentityModel.Tokens;
using New_Project_Backend.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace New_Project_Backend.Services
{
    public class TokenGeneration : ITokenGeneration
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        public TokenGeneration(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
        }

        public string CreateToken(UserDetails userDetails)
        {
            var creds = new SigningCredentials(_key, SecurityAlgorithms.Sha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Email, userDetails.Email),
                    new Claim(ClaimTypes.NameIdentifier, userDetails.User_id.ToString()),
                    new Claim(ClaimTypes.Role, userDetails.RoleName.ToString()),
                    new Claim(ClaimTypes.Name, userDetails.Username),

                    //claims.Add(new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(GetPrivileges(user.UserPkId))));
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = creds,
                Issuer = _config["Token:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
