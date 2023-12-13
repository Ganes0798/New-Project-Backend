using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Project.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Helper
{
    public class MigrationManager
    {
        IConfiguration Configuration;

        public MigrationManager(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async void StartMigration(bool seedDB)
        {
            using (ProjectDbContext dbContext = new ProjectDbContext(Configuration))
            {
                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

                if(pendingMigrations.Count() != 0)
                {
                    Console.WriteLine($@"{DateTime.Now} Starting migrations...");
                    foreach(var pendingMigration in pendingMigrations)
                    {
                        await dbContext.GetService<IMigrator>().MigrateAsync(pendingMigration);
                    }

                    var lastAppliedMigration = (await dbContext.Database.GetAppliedMigrationsAsync()).Last();
                    Console.WriteLine(@$"{DateTime.Now} Migration completed...");
                    Console.WriteLine(@$"{DateTime.Now} You are on schema version: {lastAppliedMigration}");
                }
                else
                {
                    Console.WriteLine(@$"{DateTime.Now} No migrations to apply.");
                }
                //DbSeeder.SeedDB(Configuration, dbContext);

            }
            Console.WriteLine(@$"{DateTime.Now} Press Ctrl+C to terminate this process.");
        }
    }
}
