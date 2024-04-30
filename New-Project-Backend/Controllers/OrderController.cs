using Microsoft.AspNetCore.Mvc;
using New_Project_Backend.Model;
using Project.Core.CustomModels;
using Project.Core.Data;
using static Project.Core.Enums.CommonEnums;

namespace New_Project_Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : BaseController
	{
		public OrderController(IConfiguration config) : base(config)
		{

		}


		[HttpGet]
		public ActionResult Get([FromQuery] long? id)
		{
			var _userDetails = GetCurrentUserDetail();
			try
			{
				using (ExtendedProjectDbContext dbContext = new ExtendedProjectDbContext(c_config))
				{
					var _getOrders = dbContext.Orders.Where(xy => ((id.HasValue ? (xy.Id == id) : true) && (xy.DataState == RecordState.Active)))
													  .OrderBy(xy => xy.Id)
													  .Select(ab => new Order()
													  {
														  Id = ab.Id,
														  product = new Product()
														  {
															  Id = ab.product.Id,
															  ProductName = ab.product.ProductName,
															  ProductDescription = ab.product.ProductDescription,
															  ProductQuantity = ab.product.ProductQuantity
														  },
														  register = new Register()
														  {
															  Id = ab.register.Id,
															  FirstName = ab.register.FirstName,
															  LastName = ab.register.LastName,
															  Email = ab.register.Email,
														  },
														  CreatedOn = ab.CreatedOn,
														  ModifiedOn = ab.ModifiedOn

													  }).ToList();

					if(_getOrders == null || _getOrders.Count == 0)
					{
						return SendErrorMessage(ErrorCodes.EmptyOrderList);
					}
					else
					{
						if(id.HasValue)
						{
							return Ok(new ResponseBodyResource<Order>()
							{
								Result = _getOrders[0]
							});
						}
						else
						{
							return Ok(new ResponseBodyResource<List<Order>>()
							{
								Result = _getOrders
							});
						}
					}
				}
			}
			catch(Exception)
			{
				throw;
			}
		}



		[HttpPost]
		public ActionResult Add([FromBody] CreateOrder order)
		{
			try
			{
				using(ExtendedProjectDbContext dbContext = new ExtendedProjectDbContext(c_config))
				{
					var _newOrder = new Order()
					{
						ProductFkId = order.ProductFkId,
						UserFkId = order.UserFkId
					};
					using(var transaction = dbContext.Database.BeginTransaction())
					{
						dbContext.Orders.Add(_newOrder);
						dbContext.SaveChanges();
						transaction.Commit();

						return Ok(new ResponseBodyResource<Order>()
						{
							Message = ErrorCodes.OrderPlacedSuccessfully.ToString(),
							Result = _newOrder
						});
					}
				}
			}
			catch(Exception)
			{
				throw;
			}
		}
	}
}
