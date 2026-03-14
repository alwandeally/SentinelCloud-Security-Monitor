using SentinelCloud.Core.Entities;
using SentinelCloud.Core.Interfaces;

namespace SentinelCloud.Infrastructure.Parsing;

public class CsvLogParser : ILogParser
{
    public async Task<List<LogEvent>> ParseAsync(Stream fileStream, CancellationToken cancellationToken = default)
    {
        var logEvents = new List<LogEvent>();

        using var reader = new StreamReader(fileStream);

        await reader.ReadLineAsync(); // skip header

        string? line;

        while ((line = await reader.ReadLineAsync()) != null)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split(',');

            if (parts.Length < 6)
                continue;

            if (!DateTime.TryParse(parts[0], out var timestamp))
                continue;

            var logEvent = new LogEvent
            {
                Timestamp = timestamp,
                IpAddress = parts[1],
                Username = parts[2],
                EventType = parts[3],
                Status = parts[4],
                Source = parts[5],
                RawData = line
            };

            logEvents.Add(logEvent);
        }

        return logEvents;
    }
}
