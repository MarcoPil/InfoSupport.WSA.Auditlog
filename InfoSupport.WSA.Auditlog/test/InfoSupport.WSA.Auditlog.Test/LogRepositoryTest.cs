using InfoSupport.WSA.Logging.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using InfoSupport.WSA.Logging.DAL;
using Microsoft.Extensions.DependencyInjection;
using DAL;
using InfoSupport.WSA.Logging;

public class LogRepositoryTest
{
    private static DbContextOptions<LoggerContext> CreateNewContextOptions()
    {
        // Create a fresh service provider, and therefore a fresh 
        // InMemory database instance.
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        // Create a new options instance telling the context to use an
        // InMemory database and the new service provider.
        var builder = new DbContextOptionsBuilder<LoggerContext>();
        builder.UseInMemoryDatabase()
               .UseInternalServiceProvider(serviceProvider);

        return builder.Options;
    }

    [Fact]
    public void AddEntry_to_LogRepository()
    {
        // Arrange
        var options = CreateNewContextOptions();
        var newEntry = new LogEntry();
        newEntry.RoutingKey = "Eventlogging.Test.NumberSentEvent";
        newEntry.EventType = "InfoSupport.WSA.Eventlogging.Test.NumberSentEvent";
        newEntry.Timestamp = 1234567;
        newEntry.EventJson = $"{{\"Number\":4,\"RoutingKey\":\"{newEntry.RoutingKey}\",\"Timestamp\":{newEntry.Timestamp}}}";

        ILogRepository target = new LogRepository(options);

        // Act
        target.AddEntry(newEntry);

        using (var context = new LoggerContext(options))
        {
            var entry = context.LogEntries.First(e => e.Timestamp == 1234567);

            Assert.True(entry.ID > 0);
            Assert.Equal(1234567, entry.Timestamp);
            Assert.Equal("Eventlogging.Test.NumberSentEvent", entry.RoutingKey);
            Assert.Equal("InfoSupport.WSA.Eventlogging.Test.NumberSentEvent", entry.EventType);
            Assert.Equal($"{{\"Number\":4,\"RoutingKey\":\"{newEntry.RoutingKey}\",\"Timestamp\":{newEntry.Timestamp}}}", entry.EventJson);
        }
    }
}