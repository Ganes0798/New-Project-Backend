using System.ComponentModel.DataAnnotations;

namespace New_Project_Backend.Model
{
	public class Login
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string? Username { get; set; }

		[Required]

		public string? Password { get; set; }

		[Required]
		public string? Email { get; set; }

		public bool termAccept { get; set; }
	}

	public class Signin
	{
		[Key]
		public int id { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		public string? Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string? Password { get; set; }

		public string? AccessToken { get; set; }
		public bool Rememberme { get; set; }
	}
}
