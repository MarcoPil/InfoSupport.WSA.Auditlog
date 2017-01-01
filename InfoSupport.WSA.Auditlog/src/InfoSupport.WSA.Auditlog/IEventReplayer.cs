using InfoSupport.WSA.Logging.Model;

namespace InfoSupport.WSA.Logging
{
    public interface IEventReplayer
    {
        string ExchangeName { get;  set; }
        void ReplayLogEntry(LogEntry logEntry);
    }
}