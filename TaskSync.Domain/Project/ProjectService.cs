using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket;

namespace TaskSync.Domain.Project;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly ITicketService _ticketService;

    public ProjectService(IProjectRepository projectRepository, ITicketService ticketService)
    {
        _projectRepository = projectRepository;
        _ticketService = ticketService;
    }

    public async Task<Project> CreateProjectAsync(string title, string? description, ProjectVisibility? visibility)
    {
        var project = await _projectRepository.CreateAsync(title, description, visibility);
        return project;
    }

    public async Task<Project?> GetProjectByIdAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        return project;
    }

    public async Task<PagedResult<Project>> GetProjectsAsync(int pageNumber, int pageSize)
    {
        var projects = await _projectRepository.GetAllAsync(pageNumber, pageSize);
        return projects;
    }

    public async Task DeleteProjectAsync(int id)
    {
        await _projectRepository.DeleteByIdAsync(id);
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

        return await _ticketService.GetTicketsByProjectIdAsync(projectId, pageNumber, pageSize);
    }
}

