using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TaskSync.Controllers.Response;
using TaskSync.Domain.Project;
using TaskSync.Domain.Project.Commands;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket;

namespace TaskSync.Controllers;

[Authorize]
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<ProjectResponse>> CreateProject(
        [FromBody] CreateProjectCommand req
    )
    {
        var item = await _projectService.CreateProjectAsync(req);
        return CreatedAtAction(
            nameof(GetProjectById),
            new { id = item.Id },
            new ProjectResponse(item));
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> DeleteProject([FromRoute] int id)
    {
        // TODO Permissions: Who can delete project? 

        await _projectService.DeleteProjectAsync(id);

        return NoContent();
    }

    [HttpGet]
    [Route("{projectId}/backlog")]
    public async Task GetBacklog([FromRoute] int projectId)
    {
        // TODO
    }
    
    [HttpGet]
    [Route("{id}/tickets")]
    public async Task<PagedResult<TicketResponse>> GetProjectTickets(
        [FromRoute] int id,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50
    )
    {
        PagedResult<TicketModel> pagedResult = await _projectService.GetProjectTicketsAsync(id, pageNumber, pageSize);
        return new PagedResult<TicketResponse>
        {
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize,
            Items = pagedResult.Items.Select(item => new TicketResponse(item)),
            Total = pagedResult.Total
        };
    }

    [HttpPost]
    [Route("{projectId}/team")]
    public async Task<ActionResult> AssignTeamMembers(
        [FromRoute] int projectId,
        [FromBody] AssignTeamMembersCommand command)
    {
        // TODO Permissions: Who can assign team members?
        await _projectService.AssignTeamMembersAsync(projectId, command);
        return NoContent();
    }
    
    [HttpPatch]
    [Route("{projectId}")]
    public async Task<Result<bool>> UpdateProject( 
        [FromRoute] int projectId, 
        [FromBody] UpdateProjectCommand updateProjectCommand)
    {
        return await _projectService.UpdateProjectAsync(projectId, updateProjectCommand);
    }
}
