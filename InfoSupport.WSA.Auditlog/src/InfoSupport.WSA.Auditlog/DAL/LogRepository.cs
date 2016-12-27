using System;
using System.Collections.Generic;
using InfoSupport.WSA.Logging;
using InfoSupport.WSA.Logging.DAL;
using InfoSupport.WSA.Logging.Model;
using Model;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class LogRepository : ILogRepository
    {
        private readonly DbContextOptions _options;

        public LogRepository(DbContextOptions options)
        {
            _options = options;
        }

        public void AddEntry(LogEntry entry)
        {
            using (var context = new LoggerContext(_options))
            {
                context.LogEntries.Add(entry);
                context.SaveChanges();
            }
        }

        public IEnumerable<LogEntry> FindEntriesBy(LogEntryCriteria criteria)
        {
            throw new NotImplementedException();
        }
    }
}