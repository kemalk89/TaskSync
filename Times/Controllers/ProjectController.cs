using Microsoft.AspNetCore.Mvc;
using Times.Controllers.Request;
using Times.Controllers.Response;
using Times.Domain.Project;

namespace Times.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ProjectController : ControllerBase
{

    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<IEnumerable<ProjectResponse>> GetTickets()
    {
        IEnumerable<Project> items = await _projectService.GetProjectsAsync();
        return items.Select(item => new ProjectResponse(item));
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<TicketResponse>> GetProjectById([FromRoute] int id)
    {
        var item = await _projectService.GetProjectByIdAsync(id);
        if (item == null)
        {
            return NotFound();
        }
        return Ok(new ProjectResponse(item));
    }

    [HttpPost]
    public async Task<ActionResult<ProjectResponse>> CreatePost(
        [FromBody] CreateProjectRequest req
    )
    {
        var item = await _projectService.CreateProjectAsync(req.Title, req.Description);
        return CreatedAtAction(
            nameof(GetProjectById),
            new { id = item.Id },
            new ProjectResponse(item));
    }
}
