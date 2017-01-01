using InfoSupport.WSA.Infrastructure;
using InfoSupport.WSA.Logging.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfoSupport.WSA.Logging
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DbContextOptions<LoggerContext> dbOptions = ReadDatabaseConfiguration();
            BusOptions busOptions = ReadBusConfiguration();

            using (var auditlog = new AuditlogEventListener(new LogRepository(dbOptions), busOptions))
            {
                auditlog.Start();
                Console.WriteLine("Auditlog is listening to Events...");

                KeepApplicationAlive();
            }
            Console.WriteLine("Auditlog has stopped listening to Events...");
        }

        private static BusOptions ReadBusConfiguration()
        {
            var busOptions = new BusOptions();
            Console.WriteLine("Configuring Eventbus:");
            Console.WriteLine(busOptions.ToString());
            return busOptions;
        }

        public static EventWaitHandle toStop = new AutoResetEvent(false);

        private static DbContextOptions<LoggerContext> ReadDatabaseConfiguration()
        {
            var config = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json")
                                .Build();

            string connectionString = config.GetConnectionString("AuditLogDatabase");
            var dbOptions = new DbContextOptionsBuilder<LoggerContext>()
                                .UseSqlServer(connectionString)
                                .Options;
            Console.WriteLine($"Configuring Auditlog Database: {connectionString}");
            return dbOptions;
        }

        private static void KeepApplicationAlive()
        {
            if (Environment.GetEnvironmentVariable("WSA_AUDITLOG_ENV") == "console")
            {
                Console.WriteLine("(Press any key to quit)");
                Console.ReadKey();
            }
            else
            {
                toStop.WaitOne();
            }
        }

        public static void Stop()
        {
            toStop.Set();
        }
    }
}
