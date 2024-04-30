

using static Project.Core.Enums.CommonEnums;

namespace Project.Core.Model
{
	public class CutomResults
	{
		public class UserR
		{
			public long Id { get; set; }
			public string Email { get; set; } = string.Empty;
			public string FirstName { get; set; } = string.Empty;
			public string? LastName { get; set; }
			public string UserRole { get; set; }
			public DateTime ModifiedOn { get; set; }
		}
	}
}
