using SentinelCloud.Core.Entities;

namespace SentinelCloud.Core.Interfaces;

public interface ILogParser
{
    Task<List<LogEvent>> ParseAsync(Stream fileStream, CancellationToken cancellationToken = default);
}
