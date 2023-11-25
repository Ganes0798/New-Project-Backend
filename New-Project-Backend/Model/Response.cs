namespace New_Project_Backend.Model
{
	public class Response : Services.UserDetails
	{
        public string Password { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;


    }
}
