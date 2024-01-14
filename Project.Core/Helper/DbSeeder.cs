using Microsoft.Extensions.Configuration;
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
                //if(dbContext.organization.Any())
                //{
                //    Console.WriteLine("Database Already Seeded");
                //}
                Console.WriteLine("Seeding DB...");

				List<Register> _addUsers = new List<Register>();
				Register _newUser = InitializeNewUser("SuperAdmin", "superadmin@g2met.com", Roles.SuperAdmin);
				_addUsers.Add(_newUser);

				_newUser = InitializeNewUser("Admin", "admin@g2met.com", Roles.Admin);
				_addUsers.Add(_newUser);

				_newUser = InitializeNewUser("user", "user@g2met.com", Roles.User);
				_addUsers.Add(_newUser);

				dbContext.Registration.AddRange(_addUsers);
				dbContext.SaveChanges();
				Console.WriteLine("User seeded successfully...");


			}
        }
		private static Register InitializeNewUser(string firstName, string email, Roles role)
		{
			return new Register()
			{
				Username = firstName,
				Email = email,
				RoleName = role,
				Password = "Ganesh1234",
			};
		}
	}
}
