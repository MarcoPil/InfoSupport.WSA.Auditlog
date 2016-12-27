using InfoSupport.WSA.Logging.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSupport.WSA.Logging.DAL
{
    public class LoggerContext : DbContext
    {
        public LoggerContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<LogEntry> LogEntries { get; set; }
    }
}
