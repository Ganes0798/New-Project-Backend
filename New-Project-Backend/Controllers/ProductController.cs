using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using New_Project_Backend.Model;
using Project.Core.CustomModels;
using Project.Core.Data;
using Project.Core.Enums;
using Project.Core.Interface;
using static Project.Core.Enums.CommonEnums;
using static Project.Core.Model.CutomResults;

namespace New_Project_Backend.Controllers
{
    [EnableCors("_allowOriginPolicy")]
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : BaseController
	{
        public ProductController(IConfiguration config) : base(config)
        {
            
        }


        [HttpGet]
        public ActionResult Get([FromQuery] long? id)
        {
            try
            {
                using(ExtendedProjectDbContext dbContext = new ExtendedProjectDbContext(c_config))
                {
                    var _getProducts = dbContext.Products.Where(xy => ((id.HasValue ? (xy.Id == id) : true) && (xy.DataState == RecordState.Active)))
                                                          .OrderBy(xy => xy.Id)
                                                          .Select(ab => new Product()
                                                          {
                                                              Id = ab.Id,
                                                              ProductName = ab.ProductName,
                                                              ProductDescription = ab.ProductDescription,
                                                              ProductImageUrl = ab.ProductImageUrl,
                                                              ProductPrice = ab.ProductPrice,
                                                              TotalProducts = ab.TotalProducts,
                                                              CategoryById = new Category()
                                                              {
                                                                  Code = ab.CategoryById.Code,
                                                                  Name = ab.CategoryById.Name
                                                              },
                                                              CategoryCode = ab.CategoryCode
                                                              
                                                          }).ToList();
					if ((_getProducts == null) || (_getProducts.Count == 0))
					{
						return SendErrorMessage(ErrorCodes.EmptyUsers);
					}
					else
					{
						if (id.HasValue)
						{

							return Ok(new ResponseBodyResource<Product>()
							{
								Result = _getProducts[0]
							});
						}
						else
						{
							return Ok(new ResponseBodyResource<List<Product>>()
							{
								Result = _getProducts
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
        public ActionResult Add([FromBody] CreateProduct _product)
        {
            try
            {
                using (ExtendedProjectDbContext dbcontext = new ExtendedProjectDbContext(c_config))
                {
                    var _newProduct = new Product()
                    {
                        ProductName = _product.ProductName,
                        ProductImageUrl = _product.ProductImageUrl,
                        ProductDescription = _product.ProductDescription,
                        ProductPrice = _product.ProductPrice,
                        TotalProducts = _product.TotalProducts,
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

		[HttpPatch]
        public ActionResult update([FromBody] CreateProduct _modifyProduct)
        {
            var _userDetails = GetCurrentUserDetail();
			try
            {

				using (ExtendedProjectDbContext dbContext = new ExtendedProjectDbContext(c_config))
                {
					var _existProduct = GetProductDetails(dbContext, _modifyProduct.Id);

					_existProduct.ProductName = _modifyProduct.ProductName;
                    _existProduct.ProductDescription = _modifyProduct.ProductDescription;
                    _existProduct.ProductImageUrl = _modifyProduct.ProductImageUrl;
                    _existProduct.ProductPrice = _modifyProduct.ProductPrice;
                    _existProduct.TotalProducts = _modifyProduct.TotalProducts;
                    _existProduct.CategoryCode = _modifyProduct.CategoryCode;

                    using(var transaction = dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            dbContext.Entry(_existProduct).State = EntityState.Modified;
                            dbContext.SaveChanges();
                            transaction.Commit();

                            return Ok(new ResponseBodyResource<CreateProduct>
                            {
                                Message = ErrorCodes.ProductUpdatedSuccessfully.ToString(),
                                Result = _modifyProduct
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

        [HttpDelete("{id:long}")]
        public ActionResult Delete(long id)
        {
			var _userDetails = GetCurrentUserDetail();
            try
            {
                using (ExtendedProjectDbContext dbContext = new ExtendedProjectDbContext(c_config))
                {
                    var _existProduct = GetProductDetails(dbContext, id);
                    if (_existProduct == null)
                    {
                        return SendErrorMessage(ErrorCodes.NoProductsAvailable);
                    }

                    _existProduct.DataState = RecordState.Deleted;
                    _existProduct.ModifiedOn = DateTime.UtcNow;

                    using (var transaction = dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            dbContext.Entry(_existProduct).State = EntityState.Modified;
                            dbContext.SaveChanges();
                            transaction.Commit();

                            return Ok(new ResponseBodyResource<Product>()
                            {
                                Message = ErrorCodes.ProductDeletedSuccessfully.ToString()
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
            catch (Exception ex)
            {
                throw;
            }
		}

        [HttpGet("Category")]
        public ActionResult GetCategory([FromQuery] long? id)
        {
            try
            {
                using(ExtendedProjectDbContext dbContext = new ExtendedProjectDbContext(c_config))
                {
					var _categoryDetails = dbContext.categories.Where(xy => ((id.HasValue ? (xy.Id == id) : true) &&
																 (xy.DataState == RecordState.Active)))
																 .OrderBy(xy => xy.Id)
																 .Select(xy => new Category()
																 {
                                                                     Id = xy.Id,
                                                                     Name = xy.Name,
                                                                     Code = xy.Code
																	
																 }).ToList();
					if ((_categoryDetails == null) || (_categoryDetails.Count == 0))
					{
						return SendErrorMessage(ErrorCodes.EmptyUsers);
					}
					else
					{
						if (id.HasValue)
						{

							return Ok(new ResponseBodyResource<Category>()
							{
								Result = _categoryDetails[0]
							});
						}
						else
						{
							return Ok(new ResponseBodyResource<List<Category>>()
							{
								Result = _categoryDetails
							});
						}
					}
				}
            }
            catch(Exception ex)
            {
                throw;
            }
        }
       


    }
}
