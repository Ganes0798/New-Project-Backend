using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project.Core.Enums.CommonEnums;

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

		public long UserFkId { get; set; }

		public string ProductName { get; set; } = string.Empty;

		public string ProductDescription { get; set; } = string.Empty;

		public string ProductImageUrl { get; set; } = string.Empty;

		public int ProductQuantity { get; set; }

		public long CategoryCode { get; set; }
	}

	public class CreateOrder : BaseTable
	{
		[Key]
		public long Id { get; set; }

		public long ProductFkId { get; set; }

		public long UserFkId { get; set; }
	}
}
