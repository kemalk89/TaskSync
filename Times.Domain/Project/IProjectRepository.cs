namespace Times.Domain.Project;

public interface IProjectRepository
{
    Task<Project> CreateAsync(string title, string? description);
    Task<Project?> GetByIdAsync(int id);
    Task<IEnumerable<Project>> GetAllAsync();
}
