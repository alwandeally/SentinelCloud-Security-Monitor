using System.Collections.Generic;
using System.Threading.Tasks;
using SentinelCloud.Core.Entities;

namespace SentinelCloud.Core.Interfaces;

public interface IThreatDetectionService
{
    Task<List<SecurityAlert>> AnalyzeAsync(List<LogEvent> events);
}
