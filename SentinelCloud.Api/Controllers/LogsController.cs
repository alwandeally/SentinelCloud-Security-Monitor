using Microsoft.AspNetCore.Mvc;
using SentinelCloud.Core.Interfaces;
using SentinelCloud.Infrastructure.Data;

namespace SentinelCloud.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogsController : ControllerBase
{
    private readonly ILogParser _logParser;
    private readonly IThreatDetectionService _threatService;
    private readonly AppDbContext _db;

    public LogsController(
        ILogParser logParser,
        IThreatDetectionService threatService,
        AppDbContext db)
    {
        _logParser = logParser;
        _threatService = threatService;
        _db = db;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        using var stream = file.OpenReadStream();

        var logEvents = await _logParser.ParseAsync(stream);
        var alerts = await _threatService.AnalyzeAsync(logEvents);

        _db.LogEvents.AddRange(logEvents);
        _db.SecurityAlerts.AddRange(alerts);
        await _db.SaveChangesAsync();

        return Ok(new
        {
            eventsProcessed = logEvents.Count,
            alertsDetected = alerts.Count
        });
    }
}