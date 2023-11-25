namespace New_Project_Backend.Services
{
    public interface ITokenGeneration
    {
        string CreateToken(UserDetails userDetails);
    }

    public class UserDetails
    {
        public string Email { get; set; }
        public long User_id { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }

        public bool termAccept { get; set; }
    }
}