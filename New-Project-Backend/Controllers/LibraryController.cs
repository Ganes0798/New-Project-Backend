using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using New_Project_Backend.Model;
using Project.Core.CustomModels;
using Project.Core.Data;
using static Project.Core.Enums.CommonEnums;

namespace New_Project_Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LibraryController : BaseController
	{
		public LibraryController(IConfiguration config) : base(config)
		{

		}

		[HttpGet]
		public ActionResult Get([FromQuery] long? id)
		{
			try
			{
				using (ExtendedProjectDbContext dbContext = new ExtendedProjectDbContext(c_config))
				{
					var _getLibraryBooks = dbContext.library.Where(xy => ((id.HasValue ? (xy.Id == id) : true) && (xy.DataState == RecordState.Active)))
														  .OrderBy(xy => xy.Id)
														  .Select(ab => new LibraryBooks()
														  {
															  Id = ab.Id,
															  BookName = ab.BookName,
															  BookAuthor = ab.BookAuthor,
															  BookSelfNumber = ab.BookSelfNumber,
															  LibraryHandlerName = ab.LibraryHandlerName
															 
														  }).ToList();
					if ((_getLibraryBooks == null) || (_getLibraryBooks.Count == 0))
					{
						return SendErrorMessage(ErrorCodes.EmptyUsers);
					}
					else
					{
						if (id.HasValue)
						{

							return Ok(new ResponseBodyResource<LibraryBooks>()
							{
								Result = _getLibraryBooks[0]
							});
						}
						else
						{
							return Ok(new ResponseBodyResource<List<LibraryBooks>>()
							{
								Result = _getLibraryBooks
							});
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		[HttpPost]

		public ActionResult Add([FromBody] CreateLibraryBooks _libary)
		{
			try
			{
				using(ExtendedProjectDbContext dbContext = new ExtendedProjectDbContext(c_config))
				{
					var _newBooks = new LibraryBooks()
					{
						BookName = _libary.BookName,
						BookAuthor = _libary.BookAuthor,
						BookSelfNumber = _libary.BookSelfNumber,
						LibraryHandlerName = _libary.LibraryHandlerName,
					};

					using (var transaction = dbContext.Database.BeginTransaction())
					{
						try
						{
							dbContext.library.Add(_newBooks);
							dbContext.SaveChanges();
							transaction.Commit();

							return Ok(new ResponseBodyResource<LibraryBooks>()
							{
								Message = ErrorCodes.BookAddedSuccessfully.ToString(),
								Result = _newBooks
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
			catch(Exception)
			{
				throw;
			}
		}


		[HttpPatch]
		public ActionResult update([FromBody] CreateLibraryBooks _modifyLibraryBooks)
		{
			var _userDetails = GetCurrentUserDetail();
			try
			{

				using (ExtendedProjectDbContext dbContext = new ExtendedProjectDbContext(c_config))
				{
					var _existLibraryBooks = GetLibraryBooksDetails(dbContext, _modifyLibraryBooks.Id);

					_existLibraryBooks.BookName = _modifyLibraryBooks.BookName;
					_existLibraryBooks.BookAuthor = _modifyLibraryBooks.BookAuthor;
					_existLibraryBooks.BookSelfNumber = _modifyLibraryBooks.BookSelfNumber;
					_existLibraryBooks.LibraryHandlerName = _modifyLibraryBooks.LibraryHandlerName;
					
					using (var transaction = dbContext.Database.BeginTransaction())
					{
						try
						{
							dbContext.Entry(_existLibraryBooks).State = EntityState.Modified;
							dbContext.SaveChanges();
							transaction.Commit();

							return Ok(new ResponseBodyResource<CreateLibraryBooks>
							{
								Message = ErrorCodes.BooksUpdatedSuccessfully.ToString(),
								Result = _modifyLibraryBooks
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

		[HttpPost("FormData")]
		public ActionResult FormDataAdd([FromBody] CreateFormData _formData)
		{
			try
			{
				using (ExtendedProjectDbContext dbContext = new ExtendedProjectDbContext(c_config))
				{
					var _newdata = new FormData()
					{
						Name = _formData.Name,
						Email = _formData.Email,
						PhoneNumber = _formData.PhoneNumber,
						Description = _formData.Description,
					};

					using (var transaction = dbContext.Database.BeginTransaction())
					{
						try
						{
							dbContext.formdata.Add(_newdata);
							dbContext.SaveChanges();
							transaction.Commit();

							return Ok(new ResponseBodyResource<FormData>()
							{
								Message = ErrorCodes.BookAddedSuccessfully.ToString(),
								Result = _newdata
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


		[HttpDelete("{id:long}")]
		public ActionResult Delete(long id)
		{
			var _userDetails = GetCurrentUserDetail();
			try
			{
				using (ExtendedProjectDbContext dbContext = new ExtendedProjectDbContext(c_config))
				{
					var _existLibraryBooks = GetLibraryBooksDetails(dbContext, id);
					if (_existLibraryBooks == null)
					{
						return SendErrorMessage(ErrorCodes.NoLibraryBooksAvailable);
					}

					_existLibraryBooks.DataState = RecordState.Deleted;
					_existLibraryBooks.ModifiedOn = DateTime.UtcNow;

					using (var transaction = dbContext.Database.BeginTransaction())
					{
						try
						{
							dbContext.Entry(_existLibraryBooks).State = EntityState.Modified;
							dbContext.SaveChanges();
							transaction.Commit();

							return Ok(new ResponseBodyResource<Product>()
							{
								Message = ErrorCodes.BookDeletedSuccessfully.ToString()
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
	}
}
