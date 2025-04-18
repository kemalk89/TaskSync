using TaskSync.Domain.Project.Commands;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket;

namespace TaskSync.Domain.Project;

public interface IProjectService
{
    Task<Project> CreateProjectAsync(CreateProjectCommand command);
    Task<Project?> GetProjectByIdAsync(int id);
    Task<PagedResult<Project>> GetProjectsAsync(int pageNumber, int pageSize);
    Task<PagedResult<TicketModel>> GetProjectTicketsAsync(int projectId, int pageNumber, int pageSize);
    Task DeleteProjectAsync(int id);
    Task AssignTeamMembersAsync(int projectId, AssignTeamMembersCommand command);
}
