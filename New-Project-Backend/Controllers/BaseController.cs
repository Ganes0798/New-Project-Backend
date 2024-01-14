using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Core.Interface;
using System.Security.Claims;

namespace New_Project_Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BaseController : ControllerBase
	{
		protected readonly IConfiguration c_config;

		public BaseController(IConfiguration config)
		{
			c_config = config;
		}


		protected UserDetails GetCurrentUserDetail()
		{
			var claimsPrincipal = User.Identity;
			string _email = User.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault();
			string _Id = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();
			string _role = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();
			string _name = User.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).FirstOrDefault();

			return new UserDetails() { Id = Convert.ToInt64(_Id), RoleName = _role, UserName = _name, Email = _email };
		}
	}
}
