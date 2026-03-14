using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SentinelCloud.Infrastructure.Data;

namespace SentinelCloud.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlertsController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public AlertsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAlerts()
    {
        var alerts = await _dbContext.SecurityAlerts
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return Ok(alerts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAlertById(int id)
    {
        var alert = await _dbContext.SecurityAlerts
            .FirstOrDefaultAsync(a => a.Id == id);

        if (alert == null)
        {
            return NotFound(new { message = "Alert not found" });
        }

        return Ok(alert);
    }

    [HttpPatch("{id}/resolve")]
    public async Task<IActionResult> ResolveAlert(int id)
    {
        var alert = await _dbContext.SecurityAlerts.FindAsync(id);

        if (alert == null)
        {
            return NotFound(new { message = "Alert not found." });
        }

        alert.IsResolved = true;
        await _dbContext.SaveChangesAsync();

        return Ok(new
        {
            message = "Alert resolved successfully",
            alertId = alert.Id,
            isResolved = alert.IsResolved
        });
    }
}