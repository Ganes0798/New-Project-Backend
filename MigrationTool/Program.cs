using Microsoft.Extensions.Configuration;
using Project.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrationTool
{
    class Program
    {
        static ManualResetEvent _quitEvent = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("----------------------");
                Console.WriteLine("--IMS Migration Tool--");
                Console.WriteLine("----------------------");

                bool seed = false;

                if (args.Length > 0 && args[0] == "Seed")
                {
                    Console.WriteLine($@"{DateTime.Now} Database Will be Seeded...");
                    seed = true;
                }
                var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true);
                var config = builder.Build();
                Console.CancelKeyPress += (sender, eArgs) =>
                {
                    _quitEvent.Set();
                    eArgs.Cancel = true;
                };

                MigrationManager migrationManager = new MigrationManager(config);
                migrationManager.StartMigration(seed);


                _quitEvent.WaitOne();
            }
            catch (Exception e)
            {
                Console.WriteLine(@$"{DateTime.Now} Exception:" + e.Message);
            }

        }
    }
}
