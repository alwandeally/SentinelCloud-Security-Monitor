using SentinelCloud.Core.Enums;

namespace SentinelCloud.Core.Entities;

public class SecurityAlert
{
    public int Id { get; set; }
    public AlertType AlertType { get; set; }
    public AlertSeverity Severity { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int RelatedEventCount { get; set; }
    public bool IsResolved { get; set; }
}
