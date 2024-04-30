using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using New_Project_Backend.Model;
using Project.Core.CustomModels;
using Project.Core.Data;
using Project.Core.Enums;

namespace New_Project_Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : BaseController
	{
        public ProductController(IConfiguration config) : base(config)
        {
            
        }



        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Add([FromBody] CreateProduct _product)
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
                        CategoryCode = _product.CategoryCode
                        
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

		[Authorize(Roles = "Admin")]
		[HttpPatch]
        public ActionResult update([FromBody] Product _modifyProduct)
        {
            var _userDetails = GetCurrentUserDetail();
			try
            {

				using (ExtendedProjectDbContext dbContext = new ExtendedProjectDbContext(c_config))
                {
					var _existProduct = GetProductDetails(dbContext, _modifyProduct.Id);

					_existProduct.ProductName = _modifyProduct.ProductName;
                    _existProduct.ProductDescription = _modifyProduct.ProductDescription;
                    _existProduct.ProductQuantity = _modifyProduct.ProductQuantity;
                    _existProduct.CategoryCode = _modifyProduct.CategoryCode;
                    _existProduct.UserFkId = _modifyProduct.UserFkId;

                    using(var transaction = dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            dbContext.Entry(_existProduct).State = EntityState.Modified;
                            dbContext.SaveChanges();
                            transaction.Commit();
                        }
                        catch(Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                    return SendSuccessMessage(ErrorCodes.ProductsAddedSuccessfully);
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

       


    }
}
