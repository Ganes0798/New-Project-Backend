using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using New_Project_Backend.Service;
using Project.Core.Interface;
using System.Text;

namespace New_Project_Backend.Extensions
{
	public static class ApplicationExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
		{
			//services.AddDbContext<AIPOContext>(optionsAction: options =>
			//{
			//    options.UseNpgsql(config.GetConnectionString("Postgres"));
			//});

			services.AddScoped<ITokenService, TokenService>();

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			   .AddJwtBearer(options =>
			   {
				   options.TokenValidationParameters = new TokenValidationParameters
				   {
					   ValidateIssuerSigningKey = true,
					   ValidIssuer = config["JWT:Issuer"],
					   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecretKey"]!)),
					   ValidateIssuer = true,
					   ValidAudience = config["JWT:Audience"],
					   ValidateAudience = true
				   };
			   });

			services.AddAuthorization();

			return services;
		}
	}
}
