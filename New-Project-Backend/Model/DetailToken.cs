using Project.Core.CustomModels;
using Project.Core.Interface;

namespace New_Project_Backend.Model
{
	public class DetailToken : UserDetails
	{
		public string Password { get; set; } = string.Empty;
		public string Token { get; set; } = string.Empty;


	}



	//public class AppSettings
	//{

	//}
}
