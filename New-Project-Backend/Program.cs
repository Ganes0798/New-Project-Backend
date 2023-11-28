using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using New_Project_Backend.Data;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var config = builder.Configuration;


//Authentications

//builder.Services.AddAuthentication(x =>
//{
//	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//	x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(x =>
//{
//	x.TokenValidationParameters = new TokenValidationParameters
//	{
//		ValidIssuer = config["JwtSettings:Issuer"],
//		ValidAudience = config["JwtSettings:Audience"],
//		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!)),
//		ValidateIssuer = true,
//		ValidateAudience = true,
//		ValidateLifetime = true,
//		ValidateIssuerSigningKey = true
//	};
//});


//Added Cors For All Websites
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy =>
	{
		policy.AllowAnyOrigin()
		.AllowAnyMethod()
		.AllowAnyHeader();
	});
});

//Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
