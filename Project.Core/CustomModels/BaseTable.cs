using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Project.Core.Enums.CommonEnums;

namespace Project.Core.CustomModels
{
	public class BaseTableCreated
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[JsonIgnore]
		public DateTime CreatedOn { get; set; }
	}

	public class BaseTableModified : BaseTableCreated
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[JsonIgnore]
		public DateTime ModifiedOn { get; set; }
	}

	public class BaseTable : BaseTableModified
	{

		[JsonIgnore]
		public RecordState DataState { get; set; }
	}


	public class Register : BaseTable
	{
		[Key]
		public long Id { get; set; }

		[Required]
		public string FirstName { get; set; } = string.Empty;

		[AllowNull]
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

		[Required]
		public Roles RoleName { get; set; }
	}

	public class Login
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		[JsonPropertyName("email")]
		public string Email { get; set; } = string.Empty;

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "password", Description = "Enter Valid Password")]
		[JsonPropertyName("password")]
		public string Password { get; set; } = string.Empty;
	}

	public class chngePwd
	{
		public string OldPassword { get; set; } = string.Empty;

		public string NewPassword { get; set; } = string.Empty;
	}

	

	public class Product : BaseTable
	{
		[Key]
		public long Id { get; set; }

		public string ProductName { get; set; } = string.Empty;

		public string ProductDescription { get; set; } = string.Empty;

		public string ProductImageUrl { get; set; } = string.Empty;

		public decimal ProductPrice { get; set; }
		public int TotalProducts { get; set; }

		[ForeignKey("CategoryCode")]
		public Category CategoryById { get; set; }

		public long CategoryCode { get; set; }

		public ICollection<BillingProduct> BillingProducts { get; set; }
	}

	public class Category : BaseTable
	{
		public long Id { get; set; }
		public string Name { get; set; }

		public Int16 Code { get; set; }
	}

	public class CartDetails: BaseTable
	{
		public long Id { get; set; }

		[ForeignKey("UserFkId")]
		public Register User { get; set; }

		public long UserFkId { get; set; }

		[ForeignKey("ProductFkId")]
		public Product Product { get; set; }

		public long ProductFkId { get; set; }

		public long Quantity { get; set; }

	}
    public class Billings : BaseTable
    {
        public long Id { get; set; }
        public string BillingReferenceNo { get; set; }
        public DateTime BillingDate { get; set; }

        [ForeignKey("CustomerFkId")]
        public Register Customer { get; set; }

        public long CustomerFkId { get; set; }

        public decimal TotalAmount { get; set; }
        public ICollection<BillingProduct> BillingProducts { get; set; }
    }

    public class BillingProduct : BaseTable
    {
        public long Id { get; set; }
        public long ProductId { get; set; }

        public long BillingId { get; set; }
        public Billings Billing { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class BillingRequest
    {
        public int CustomerId { get; set; }
        public string BillingReferenceNo { get; set; }
        public decimal SubTotal { get; set; }

        public decimal TotalAmount { get; set; }
        public List<BillingProductRequest> Products { get; set; }
    }

    public class BillingProductRequest
    {
        public string Name { get; set; }
        public decimal MRP { get; set; }
        public string ProductImage { get; set; }

		public string ProductDesc { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }

		public long categoryCode { get; set; }
    }



    public class EmailModel
	{
		public string Email { get; set; }
		public string? Name { get; set; }

		public string? PhoneNumber { get; set; }
	}



}