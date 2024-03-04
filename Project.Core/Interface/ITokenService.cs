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
}
