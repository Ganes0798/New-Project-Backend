using System.ComponentModel.DataAnnotations;
using static New_Project_Backend.Enums.CustomEnums;

namespace New_Project_Backend.Model
{
    public class Login
	{
		[Key]
		public int User_id { get; set; }

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

		public DateTime createdOn { get; set; }	
	}

	public class Signin
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; } = string.Empty;

        [Required]
		[DataType(DataType.Password)]
        [Display(Name = "password", Description = "Enter Valid Password")]
        public string Password { get; set; } = string.Empty;
        public bool Rememberme { get; set; }
	}
}
