using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using New_Project_Backend.Model;
using Project.Core.CustomModels;
using Project.Core.Data;

namespace New_Project_Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : BaseController
	{
        public ProductController(IConfiguration config) : base(config)
        {
            
        }




        [Authorize("Admin")]
        [HttpPost]
        public ActionResult Add([FromBody] Product _product)
        {
            try
            {
                using (ExtendedProjectDbContext dbcontext = new ExtendedProjectDbContext(c_config))
                {
                    var _newProduct = new Product()
                    {
                        ProductName = _product.ProductName,
                        ProductDescription = _product.ProductDescription,
                        ProductQuantity = _product.ProductQuantity,
                        UserFkId = _product.UserFkId,
                        Category = _product.Category
                    };

                    using (var transaction = dbcontext.Database.BeginTransaction())
                    {
                        try
                        {
                            dbcontext.Products.Add(_newProduct);
                            dbcontext.SaveChanges();
                            transaction.Commit();

                            return Ok(new ResponseBodyResource<Product>()
                            {
                                Message = ErrorCodes.ProductsAddedSuccessfully.ToString(),
                                Result = _newProduct
                            });
                        }
                        catch(Exception)
                        {
							transaction.Rollback();
							throw;
                        }
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
