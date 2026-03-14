using SentinelCloud.Core.Entities;
using SentinelCloud.Core.Enums;
using SentinelCloud.Core.Interfaces;

namespace SentinelCloud.Infrastructure.Services;

public class ThreatDetectionService : IThreatDetectionService
{
    public Task<List<SecurityAlert>> AnalyzeAsync(List<LogEvent> events)
    {
        var alerts = new List<SecurityAlert>();

        alerts.AddRange(DetectRepeatedFailedLogins(events));
        alerts.AddRange(DetectFailedThenSuccess(events));

        return Task.FromResult(alerts);
    }

    private static IEnumerable<SecurityAlert> DetectRepeatedFailedLogins(List<LogEvent> logEvents)
    {
        var failedGroups = logEvents
            .Where(x => x.Status.Equals("Failed", StringComparison.OrdinalIgnoreCase))
            .GroupBy(x => x.IpAddress);

        foreach (var group in failedGroups)
        {
            if (group.Count() >= 3)
            {
                yield return new SecurityAlert
                {
                    AlertType = AlertType.RepeatedFailedLogins,
                    Severity = AlertSeverity.Medium,
                    Title = "Repeated failed logins detected",
                    Description = $"IP {group.Key} generated {group.Count()} failed login attempts.",
                    IpAddress = group.Key,
                    CreatedAt = DateTime.UtcNow,
                    RelatedEventCount = group.Count(),
                    IsResolved = false
                };
            }
        }
    }

    private static IEnumerable<SecurityAlert> DetectFailedThenSuccess(List<LogEvent> logEvents)
    {
        var groupedByIp = logEvents
            .OrderBy(x => x.Timestamp)
            .GroupBy(x => x.IpAddress);

        foreach (var group in groupedByIp)
        {
            var events = group.ToList();
            var failedCount = 0;

            foreach (var logEvent in events)
            {
                if (logEvent.Status.Equals("Failed", StringComparison.OrdinalIgnoreCase))
                {
                    failedCount++;
                }

                if (failedCount >= 3 &&
                    logEvent.Status.Equals("Success", StringComparison.OrdinalIgnoreCase))
                {
                    yield return new SecurityAlert
                    {
                        AlertType = AlertType.FailedThenSuccess,
                        Severity = AlertSeverity.High,
                        Title = "Suspicious login success after failures",
                        Description = $"IP {group.Key} had multiple failed logins followed by a success.",
                        IpAddress = group.Key,
                        CreatedAt = DateTime.UtcNow,
                        RelatedEventCount = failedCount + 1,
                        IsResolved = false
                    };

                    break;
                }
            }
        }
    }
}