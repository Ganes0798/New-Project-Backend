using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using New_Project_Backend.Model;
using Project.Core.CustomModels;
using Project.Core.Data;
using Project.Core.Interface;
using System.Security.Claims;
using static Project.Core.Enums.CommonEnums;

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


		protected Register GetUserDetails(ExtendedProjectDbContext dbContext, long id)
		{
			return dbContext.Users.Where(xy => ((xy.DataState == RecordState.Active) &&
												(xy.Id == id))).FirstOrDefault();
		}

		protected Product GetProductDetails(ExtendedProjectDbContext dbContext, long id)
		{
			return dbContext.Products.Where(xy => ((xy.DataState == RecordState.Active) &&
												(xy.Id == id))).FirstOrDefault();
		}

		protected CartDetails GetCartDetails(ExtendedProjectDbContext dbContext, long id)
		{
			return dbContext.cart.Where(xy => ((xy.DataState == RecordState.Active) && (xy.Id == id))).FirstOrDefault();
		}
		protected LibraryBooks GetLibraryBooksDetails(ExtendedProjectDbContext dbContext, long id)
		{
			return dbContext.library.Where(xy => ((xy.DataState == RecordState.Active) && (xy.Id == id))).FirstOrDefault();
		}


		protected ObjectResult SendErrorMessage(ErrorCodes errorCode)
		{
			var failedResp = new ResponseBodyResource<string>(false, StatusCodes.Status200OK, errorCode)
			{
				Success = false
			};
			return new ObjectResult(failedResp)
			{
				ContentTypes = { "application/problem+json" },
				StatusCode = StatusCodes.Status200OK
			};
		}

		protected ObjectResult SendSuccessMessage(ErrorCodes errorCode)
		{
			var successResp = new ResponseBodyResource<string>()
			{
				Result = errorCode.ToString()
			};
			return new ObjectResult(successResp)
			{
				ContentTypes = { "application/json" },
				StatusCode = StatusCodes.Status200OK
			};
		}
	}
}
