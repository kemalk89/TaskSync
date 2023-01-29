namespace Times.Domain.Project;

public interface IProjectService
{
    Task<Project> CreateProjectAsync(string title, string? description);
    Task<Project?> GetProjectByIdAsync(int id);
    Task<IEnumerable<Project>> GetProjectsAsync();
}
