using TaskSync.Domain.Project.Commands;
using TaskSync.Domain.Project.CreateProject;
using TaskSync.Domain.Shared;

namespace TaskSync.Domain.Project;

public interface IProjectRepository
{
    Task<ProjectModel> CreateAsync(CreateProjectCommand command);
    Task<ProjectModel?> GetByIdAsync(int id);
    Task<PagedResult<ProjectModel>> GetAllAsync(int pageNumber, int pageSize);
    Task DeleteByIdAsync(int id);
    Task UpdateProjectAsync(int projectId, UpdateProjectCommand command);
    Task SaveAsync(ProjectModel projectModel);
    Task<int> CreateLabelAsync(int projectId, string text);
    Task<List<ProjectLabelModel>> GetLabelsAsync(int projectId);
}
