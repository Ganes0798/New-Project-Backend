using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project.Core.Enums.CommonEnums;
using System.Text.Json.Serialization;

namespace Project.Core.CustomModels
{
	public class CreateUsers : BaseTable
	{
		public long Id { get; set; }

		[Required]
		public string FirstName { get; set; } = string.Empty;

		public string? LastName { get; set; } = string.Empty;

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;

		[Required]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; } = string.Empty;

		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; } = string.Empty;

	}
		public class CreateProduct : BaseTable
	{
		[Key]
		public long Id { get; set; }

		public string ProductName { get; set; } = string.Empty;

		public string ProductDescription { get; set; } = string.Empty;

		public string ProductImageUrl { get; set; } = string.Empty;

		public long ProductPrice { get; set; }

		public int TotalProducts { get; set; }

		public long CategoryCode { get; set; }
	}
	public class CreateCart : BaseTable
	{

		public long UserFkId { get; set; }

		public long ProductFkId { get; set; }

		public long Quantity { get; set; }

	}

	public class CreateOrder : BaseTable
	{
		[Key]
		public long Id { get; set; }

		public long ProductFkId { get; set; }

		public long UserFkId { get; set; }
	}

	public class CreateLibraryBooks : BaseTable
	{
		public long Id { get; set; }
		public string BookName { get; set; }

		public string BookAuthor { get; set; }

		public string BookSelfNumber { get; set; }

		public string LibraryHandlerName { get; set; }
	}

	public class SearchRequest
	{
		[JsonPropertyName("searchValue")]
		public string SearchValue { get; set; }

		[JsonPropertyName("searchField")]
		public SearchFields FieldName { get; set; }
	}

	public class CreateFormData : BaseTable
	{

		public string Name { get; set; }

		public string Email { get; set; }

		public string PhoneNumber { get; set; }

		public string Description { get; set; }


	}
}
