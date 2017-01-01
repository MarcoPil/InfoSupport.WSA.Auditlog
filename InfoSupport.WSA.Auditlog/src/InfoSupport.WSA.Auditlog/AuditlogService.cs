using System;
using InfoSupport.WSA.Infrastructure;
using InfoSupport.WSA.Logging;
using InfoSupport.WSA.Logging.Model;

namespace InfoSupport.WSA.Logging
{
    public class AuditlogService : IAuditlogService
    {
        private readonly ILogRepository _logRepo;
        private readonly IEventReplayer _replayer;

        public AuditlogService(ILogRepository logRepo, IEventReplayer replayer)
        {
            _logRepo = logRepo;
            _replayer = replayer;
        }

        public void ReplayEvents(ReplayEventsCommand replayEventsCommand)
        {
            var criteria = new LogEntryCriteria
            {
                FromTimestamp = replayEventsCommand.FromTimestamp,
                ToTimestamp = replayEventsCommand.ToTimestamp,
                EventType = replayEventsCommand.EventType,
                RoutingKeyExpression = replayEventsCommand.RoutingKeyExpression,
            };

            var entries = _logRepo.FindEntriesBy(criteria);


            _replayer.ExchangeName = replayEventsCommand.ExchangeName;

            foreach (var entry in entries)
            {
                _replayer.ReplayLogEntry(entry);
            }
        }
    }
}