using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SentinelCloud.Infrastructure.Data;

namespace SentinelCloud.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _db;

    public DashboardController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var totalLogs = await _db.LogEvents.CountAsync();
        var totalAlerts = await _db.SecurityAlerts.CountAsync();
        var resolvedAlerts = await _db.SecurityAlerts.CountAsync(a => a.IsResolved);
        var unresolvedAlerts = await _db.SecurityAlerts.CountAsync(a => !a.IsResolved);
        var highSeverityAlerts = await _db.SecurityAlerts.CountAsync(a => (int)a.Severity >= 3);

        var mostAttackedIp = await _db.SecurityAlerts
            .GroupBy(a => a.IpAddress)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefaultAsync();

        return Ok(new
        {
            totalLogs,
            totalAlerts,
            resolvedAlerts,
            unresolvedAlerts,
            highSeverityAlerts,
            mostAttackedIp
        });
    }
}
