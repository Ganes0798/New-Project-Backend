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
