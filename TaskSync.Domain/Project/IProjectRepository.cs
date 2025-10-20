using TaskSync.Domain.Project.Commands;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket;

namespace TaskSync.Domain.Project;

public interface IProjectRepository
{
    Task<Project> CreateAsync(CreateProjectCommand command);
    Task<Project?> GetByIdAsync(int id);
    Task<PagedResult<Project>> GetAllAsync(int pageNumber, int pageSize);
    Task DeleteByIdAsync(int id);
    Task UpdateProjectAsync(int projectId, UpdateProjectCommand command);
    Task SaveAsync(Project project);
    Task<int> CreateLabelAsync(int projectId, string text);
    Task<List<ProjectLabelModel>> GetLabelsAsync(int projectId);
}
