using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
		[JsonIgnore]
		[Key]
		public int Id { get; set; }

		[Required]
		public string Username { get; set; } = string.Empty;

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;

		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; } = string.Empty;

		[Required]
		public Roles RoleName { get; set; }


		public bool termAccept { get; set; }
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
		public bool Rememberme { get; set; }
	}
}
