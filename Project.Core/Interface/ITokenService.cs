namespace Project.Core.Interface
{
	public interface ITokenService
	{
		string CreateToken(UserDetails userDetails);
	}

	public class UserDetails
	{
		public string Email { get; set; }
		public long Id { get; set; }
		public string UserName { get; set; }
		public string RoleName { get; set; }
	}

	public class ApplicationSettings
	{
		public string GoogleClientId { get; set; }
	}

	public class GoogleUserInfo
	{
		public string Sub { get; set; } // User ID
		public string Name { get; set; } // User's full name
		public string GivenName { get; set; } // User's first name
		public string FamilyName { get; set; } // User's last name
		public string Email { get; set; } // User's email address
		public bool EmailVerified { get; set; } // Indicates if the user's email has been verified by Google
												// You may add more properties here if needed, depending on the data returned by Google
	}
}
