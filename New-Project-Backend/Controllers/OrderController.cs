using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using New_Project_Backend.Model;
using Project.Core.CustomModels;
using Project.Core.Data;
using static Project.Core.Enums.CommonEnums;

namespace New_Project_Backend.Controllers
{
    [AllowAnonymous]
	[EnableCors("_allowOriginPolicy")]
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : BaseController
	{
		public OrderController(IConfiguration config) : base(config)
		{

		}

        [HttpPost]
        public async Task<IActionResult> SaveBilling([FromBody] BillingRequest request)
        {
            try
            {
                using (ProjectDbContext dbContext = new ProjectDbContext(c_config))
                {
                    var customer = await dbContext.Users.FirstOrDefaultAsync(c => c.Id == request.CustomerId);
                    if (customer == null)
                    {
                        return BadRequest("Customer not found");
                    }

                    // Create a new billing record
                    var billing = new Billings
                    {
                        BillingReferenceNo = request.BillingReferenceNo,
                        BillingDate = DateTime.UtcNow,
                        TotalAmount = request.TotalAmount,
                        Customer = customer,
                        BillingProducts = request.Products.Select(p => new BillingProduct
                        {
                            Product = new Product { ProductName = p.Name, ProductPrice = p.MRP, ProductImageUrl = p.ProductImage, ProductDescription = p.ProductDesc, CategoryCode = p.categoryCode },
                            Quantity = p.Quantity,
                            TotalAmount = p.TotalAmount
                        }).ToList()
                    };

                    // Add billing to the database
                    dbContext.billings.Add(billing);
                    await dbContext.SaveChangesAsync();

                    return Ok(new ResponseBodyResource<Billings>()
                    {
                        Message = ErrorCodes.OrderPlacedSuccessfully.ToString(),
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
    //protected string GenerateRandomOrderId(int length, DateTime date)
    //{
    //    const string chars = "EHOUSE1234567890";
    //    Random random = new Random();
    //    return new string(Enumerable.Repeat(chars, length)
    //        .Select(s => s[random.Next(s.Length)]).ToArray());
    //}

}
