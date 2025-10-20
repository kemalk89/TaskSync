using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TaskSync.Controllers.Response;
using TaskSync.Domain.Exceptions;
using TaskSync.Domain.Project;
using TaskSync.Domain.Project.Commands;
using TaskSync.Domain.Project.CreateProject;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket;

namespace TaskSync.Controllers.Project;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;
    protected readonly CreateProjectCommandHandler _createProjectCommandHandler;

    public ProjectController(IProjectService projectService, CreateProjectCommandHandler createProjectCommandHandler)
    {
        _projectService = projectService;
        _createProjectCommandHandler = createProjectCommandHandler;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ProjectResponse>> CreateProject(
        [FromBody] CreateProjectCommand command
    )
    {
        var result = await _createProjectCommandHandler.HandleCommandAsync(command);
        if (result.Success && result.Value != null)
        {
            return CreatedAtAction(
                nameof(GetProjectById),
                new { id = result.Value.Id },
                new ProjectResponse(result.Value));
        }

        return BadRequest(new ErrorResponse(result.Error, result.ErrorDetails));
    }
    
    [HttpGet]
    public async Task<PagedResult<ProjectResponse>> GetProjects([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
    {
        PagedResult<ProjectModel> pagedResult = await _projectService.GetProjectsAsync(pageNumber, pageSize);
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TicketResponse>> GetProjectById([FromRoute] int id)
    {
        var item = await _projectService.GetProjectByIdAsync(id);
        if (item == null)
        {
            return NotFound();
        }
        return Ok(new ProjectResponse(item));
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
    public Task GetBacklog([FromRoute] int projectId)
    {
        // TODO
        throw new NotImplementedException();
    }

    [HttpPost]
    [Route("{id}/labels")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ProjectLabelResponse>> CreateTicketLabel([FromBody] CreateProjectLabelCommand command)
    {
        var result = await _projectService.CreateTicketLabelAsync(command);
        if (result.Success)
        {
            return CreatedAtAction(
                null,
                new ProjectLabelResponse { Id = result.Value, Text = command.Text});
        }
        
        if (ResultCodes.ResultCodeValidationFailed.Equals(result.Error))
        {
            return BadRequest(result);
        }
        
        throw new DomainException("Unknown error.");
    }
    
    [HttpGet]
    [Route("{id}/labels")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<List<ProjectLabelResponse>> GetProjectLabels([FromRoute] int id)
    {
        var result = await _projectService.GetLabelsAsync(id);
        if (result.Success && result.Value != null)
        {
            return result.Value.Select(e => new ProjectLabelResponse
            {
                Id = e.Id,
                Text = e.Text
            }).ToList();   
        }

        throw new DomainException("Unexpected result.");
    }
    
    [HttpGet]
    [Route("{id}/tickets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
