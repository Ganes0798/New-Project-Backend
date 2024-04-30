using Microsoft.Extensions.Configuration;
using Project.Core.Extensions;
using Project.Core.CustomModels;
using Project.Core.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Project.Core.Enums.CommonEnums;

namespace IMS.Core.Helper
{
    public static class DbSeeder
    {
        public static void SeedDB(IConfiguration configuration, ProjectDbContext dbContext)
        {
            using (ProjectDbContext seedContext = new ProjectDbContext(configuration))
            {
				if (dbContext.Users.Any())
				{
					Console.WriteLine("Database Already Seeded");
				}
				Console.WriteLine("Seeding DB...");

				List<Category> _addCategory = new List<Category>();
				_addCategory.Add(new Category() { Code = 1, Name = "Fashion" });
				_addCategory.Add(new Category() { Code = 2, Name = "Electronics" });
				_addCategory.Add(new Category() { Code = 3, Name = "Electricals" });
				_addCategory.Add(new Category() { Code = 4, Name = "Appliances" });
				_addCategory.Add(new Category() { Code = 5, Name = "Home Needs" });
				_addCategory.Add(new Category() { Code = 6, Name = "Groceries" });

				dbContext.categories.AddRange(_addCategory);
				dbContext.SaveChanges();
				Console.WriteLine("Catogories Seeded Successfully.....");


				List<Register> _addUsers = new List<Register>();
				Register _newUser = InitializeNewUser("SuperAdmin", "", "superadmin@g2met.com", Roles.SuperAdmin);
				_addUsers.Add(_newUser);

				_newUser = InitializeNewUser("Admin", "", "admin@g2met.com", Roles.Admin);
				_addUsers.Add(_newUser);

				_newUser = InitializeNewUser("user", "", "user@g2met.com", Roles.User);
				_addUsers.Add(_newUser);

				dbContext.Users.AddRange(_addUsers);
				dbContext.SaveChanges();
				Console.WriteLine("User seeded successfully...");
			}
        }
		private static Register InitializeNewUser(string firstName, string lastName, string email, Roles role)
		{
			return new Register()
			{
				FirstName = firstName,
				LastName = lastName,
				Email = email,
				RoleName = role,
				Password = EncryptDecrypt.EncryptString("PasswordA!1234@"),
			};
		}
	}
}
