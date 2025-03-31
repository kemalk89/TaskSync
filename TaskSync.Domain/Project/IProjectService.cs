using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket;

namespace TaskSync.Domain.Project;

public interface IProjectService
{
    Task<Project> CreateProjectAsync(string title, string? description, ProjectVisibility? visibility);
    Task<Project?> GetProjectByIdAsync(int id);
    Task<PagedResult<Project>> GetProjectsAsync(int pageNumber, int pageSize);
    Task<PagedResult<TicketModel>> GetProjectTicketsAsync(int projectId, int pageNumber, int pageSize);
    Task DeleteProjectAsync(int id);
}
