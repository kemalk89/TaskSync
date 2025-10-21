using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket;
using TaskSync.Domain.Ticket.QueryTicket;

namespace TaskSync.Domain.Project.QueryProject;

public class QueryProjectCommandHandler : ICommandHandler
{
        
    private readonly IProjectRepository _projectRepository;
    private readonly QueryTicketCommandHandler _queryTicketCommandHandler;
    
    public QueryProjectCommandHandler(
        IProjectRepository projectRepository,
        QueryTicketCommandHandler queryTicketCommandHandler)
    {
        _projectRepository = projectRepository;
        _queryTicketCommandHandler = queryTicketCommandHandler;
    }

    public async Task<ProjectModel?> GetProjectByIdAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        return project;
    }

    public async Task<PagedResult<ProjectModel>> GetProjectsAsync(int pageNumber, int pageSize)
    {
        var projects = await _projectRepository.GetAllAsync(pageNumber, pageSize);
        return projects;
    }



    public async Task<PagedResult<TicketModel>> GetProjectTicketsAsync(int projectId, int pageNumber = 1, int pageSize = 0)
    {
        var project = await GetProjectByIdAsync(projectId);
        if (project == null)
        {
            return new PagedResult<TicketModel>
            {
                Total = 0,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        return await _queryTicketCommandHandler.GetTicketsByProjectIdAsync(projectId, pageNumber, pageSize);
    }

    public async Task<Result<List<ProjectLabelModel>>> GetLabelsAsync(int projectId)
    {
        var result = await _projectRepository.GetLabelsAsync(projectId);
        return Result<List<ProjectLabelModel>>.Ok(result);
    }
}