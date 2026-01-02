using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TaskSync.Controllers.Response;
using TaskSync.Domain.Exceptions;
using TaskSync.Domain.Project;
using TaskSync.Domain.Project.CreateProject;
using TaskSync.Domain.Project.AssignProjectLabel;
using TaskSync.Domain.Project.AssignTeamMembers;
using TaskSync.Domain.Project.DeleteProject;
using TaskSync.Domain.Project.QueryProject;
using TaskSync.Domain.Project.ReorderBacklogTickets;
using TaskSync.Domain.Project.UpdateProject;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket;

namespace TaskSync.Controllers.Project;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly QueryProjectCommandHandler _queryProjectCommandHandler;
    private readonly CreateProjectCommandHandler _createProjectCommandHandler;
    private readonly UpdateProjectCommandHandler _updateProjectCommandHandler;
    private readonly DeleteProjectCommandHandler _deleteProjectCommandHandler;
    private readonly AssignProjectLabelCommandHandler _assignProjectLabelCommandHandler;
    private readonly AssignTeamMembersCommandHandler _assignTeamMembersCommandHandler;
    private readonly ReorderBacklogTicketsCommandHandler _reorderBacklogTicketsCommandHandler;

    public ProjectController(
        CreateProjectCommandHandler createProjectCommandHandler, 
        AssignProjectLabelCommandHandler assignProjectLabelCommandHandler, 
        AssignTeamMembersCommandHandler assignTeamMembersCommandHandler, 
        UpdateProjectCommandHandler updateProjectCommandHandler, 
        DeleteProjectCommandHandler deleteProjectCommandHandler, 
        QueryProjectCommandHandler queryProjectCommandHandler, 
        ReorderBacklogTicketsCommandHandler reorderBacklogTicketsCommandHandler)
    {
        _createProjectCommandHandler = createProjectCommandHandler;
        _assignProjectLabelCommandHandler = assignProjectLabelCommandHandler;
        _assignTeamMembersCommandHandler = assignTeamMembersCommandHandler;
        _updateProjectCommandHandler = updateProjectCommandHandler;
        _deleteProjectCommandHandler = deleteProjectCommandHandler;
        _queryProjectCommandHandler = queryProjectCommandHandler;
        _reorderBacklogTicketsCommandHandler = reorderBacklogTicketsCommandHandler;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ProjectResponse>> CreateProject(
        [FromBody] CreateProjectCommand command
    )
    {
        var result = await _createProjectCommandHandler.HandleAsync(command);
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
        PagedResult<ProjectModel> pagedResult = await _queryProjectCommandHandler.GetProjectsAsync(pageNumber, pageSize);
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
    public async Task<ActionResult<ProjectResponse>> GetProjectById([FromRoute] int id)
    {
        var item = await _queryProjectCommandHandler.GetProjectByIdAsync(id);
        if (item == null)
        {
            return NotFound();
        }
        return Ok(new ProjectResponse(item));
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteProject([FromRoute] int id)
    {
        // TODO Permissions: Who can delete project? 

        await _deleteProjectCommandHandler.HandleAsync(id);

        return NoContent();
    }

    [HttpGet]
    [Route("{projectId}/backlog")]
    public async Task<List<TicketModel>> GetBacklog([FromRoute] int projectId, CancellationToken cancellationToken)
    {
        var result = await _queryProjectCommandHandler.GetBacklogTicketsAsync(projectId, cancellationToken);
        return result;
    }

    [HttpPost]
    [Route("{projectId}/backlog/reorder")]
    public async Task<ActionResult> ReorderBacklogTickets(
        [FromRoute] int projectId, 
        [FromBody] List<ReorderBacklogTicketCommand> ticketOrder, 
        CancellationToken cancellationToken)
    {
        var result = await _reorderBacklogTicketsCommandHandler.HandleAsync(projectId, ticketOrder, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Route("{id}/labels")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ProjectLabelResponse>> CreateTicketLabel([FromBody] AssignProjectLabelCommand command)
    {
        var result = await _assignProjectLabelCommandHandler.HandleAsync(command);
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
        var result = await _queryProjectCommandHandler.GetLabelsAsync(id);
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
        PagedResult<TicketModel> pagedResult = await _queryProjectCommandHandler.GetProjectTicketsAsync(id, pageNumber, pageSize);
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
        await _assignTeamMembersCommandHandler.HandleAsync(projectId, command);
        return NoContent();
    }
    
    [HttpPatch]
    [Route("{projectId}")]
    public async Task<Result<bool>> UpdateProject( 
        [FromRoute] int projectId, 
        [FromBody] UpdateProjectCommand updateProjectCommand)
    {
        return await _updateProjectCommandHandler.HandleAsync(projectId, updateProjectCommand);
    }
}
