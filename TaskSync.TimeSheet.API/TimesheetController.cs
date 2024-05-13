using Microsoft.AspNetCore.Mvc;

namespace TaskSync.TimeSheet.API;

[ApiController]
[Route("/api/timesheet")]
public class TimesheetController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        await Task.Delay(100);
        return Ok("Timesheet API");
    }
}