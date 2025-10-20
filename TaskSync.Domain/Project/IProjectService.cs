using TaskSync.Domain.Project.Commands;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket;

namespace TaskSync.Domain.Project;

public interface IProjectService
{
    Task<Result<Project>> CreateProjectAsync(CreateProjectCommand command);
    Task<Project?> GetProjectByIdAsync(int id);
    Task<PagedResult<Project>> GetProjectsAsync(int pageNumber, int pageSize);
    Task<PagedResult<TicketModel>> GetProjectTicketsAsync(int projectId, int pageNumber, int pageSize);
    Task DeleteProjectAsync(int id);
    Task<Result<bool>> AssignTeamMembersAsync(int projectId, AssignTeamMembersCommand command);
    Task<Result<bool>> UpdateProjectAsync(int projectId, UpdateProjectCommand updateProjectCommand);
    Task<Result<List<ProjectLabelModel>>> GetLabelsAsync(int projectId);
    Task<Result<int>> CreateTicketLabelAsync(CreateProjectLabelCommand command);
}
