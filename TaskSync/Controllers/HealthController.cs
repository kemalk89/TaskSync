using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TaskSync.Controllers;

[ApiController]
[Route("/api/health")]
public class HealthController : ControllerBase
{

    private readonly ILogger<HealthController> _logger;
    private readonly HealthCheckService _service;

    public HealthController(ILogger<HealthController> logger, HealthCheckService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var report = await _service.CheckHealthAsync();

        _logger.LogInformation($"Get Health Information: {report}");

        if (report.Status == HealthStatus.Healthy)
        {
            return Ok(report);
        }
        else
        {
            return StatusCode((int)HttpStatusCode.ServiceUnavailable, report);
        }
    }

}
