//using IMS.Core.Context;
//using IMS.Core.CustomModel;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics.Metrics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace IMS.Core.Helper
//{
//    public static class DbSeeder
//    {
//        public static void SeedDB(IConfiguration configuration, IMSDbContext dbContext)
//        {
//            using (IMSDbContext seedContext = new IMSDbContext(configuration))
//            {
//                //if(dbContext.organization.Any())
//                //{
//                //    Console.WriteLine("Database Already Seeded");
//                //}
//                Console.WriteLine("Seeding DB...");
//                string _name = "New Organizations";
//                Organizations _org = new Organizations()
//                {
//                    Name = _name,
//                    Domain = "",
//                    Email = "",
//                    PhoneNumber = "",
//                    Building = "",
//                    Street = "",
//                    Area = "",
//                    City = "",
//                    State = "",
//                    Country = "",
//                    PostalCode = "",
//                    Industry = "",
//                };
//                dbContext.organization.Add(_org);
//                dbContext.SaveChanges();
//                Console.WriteLine("Organization Seeded Successfully...");
//            }
//        }
//    }
//}
