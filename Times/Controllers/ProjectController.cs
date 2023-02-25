using Microsoft.AspNetCore.Mvc;
using Times.Controllers.Request;
using Times.Controllers.Response;
using Times.Domain.Project;
using Times.Domain.Shared;

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
    public async Task<PagedResult<ProjectResponse>> GetProjects([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
    {
        PagedResult<Project> pagedResult = await _projectService.GetProjectsAsync(pageNumber, pageSize);
        return new PagedResult<ProjectResponse>
        {
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize,
            Items = pagedResult.Items.Select(item => new ProjectResponse(item)),
            Total = pagedResult.Total
        };
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

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> DeleteProject([FromRoute] int id)
    {
        await _projectService.DeleteProjectAsync(id);

        return NoContent();
    }
}
