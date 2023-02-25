using Times.Domain.Shared;

namespace Times.Domain.Project;

public interface IProjectService
{
    Task<Project> CreateProjectAsync(string title, string? description);
    Task<Project?> GetProjectByIdAsync(int id);
    Task<PagedResult<Project>> GetProjectsAsync(int pageNumber, int pageSize);
    Task DeleteProjectAsync(int id);
}
