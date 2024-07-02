using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using New_Project_Backend.Model;
using Project.Core.CustomModels;
using Project.Core.Data;

namespace New_Project_Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CartController : BaseController
	{
		public CartController(IConfiguration config) : base(config)
		{

		}


		[HttpPost]
		public ActionResult Add([FromBody] CreateCart _cart)
		{
			try
			{
				using (ExtendedProjectDbContext dbcontext = new ExtendedProjectDbContext(c_config))
				{
					var _newCart = new CartDetails()
					{
						ProductFkId = _cart.ProductFkId,
						UserFkId = _cart.UserFkId,
						Quantity = _cart.Quantity

					};

					

					using (var transaction = dbcontext.Database.BeginTransaction())
					{
						try
						{
							dbcontext.cart.Add(_newCart);
							dbcontext.SaveChanges();
							transaction.Commit();

							return Ok(new ResponseBodyResource<CartDetails>()
							{
								Message = ErrorCodes.CartAddedSuccessfully.ToString(),
								Result = _newCart
							});
						}
						catch (Exception)
						{
							transaction.Rollback();
							throw;
						}
					}

				}
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
