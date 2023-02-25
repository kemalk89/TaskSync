using Times.Domain.Shared;

namespace Times.Domain.Project;

public interface IProjectRepository
{
    Task<Project> CreateAsync(string title, string? description);
    Task<Project?> GetByIdAsync(int id);
    Task<PagedResult<Project>> GetAllAsync(int pageNumber, int pageSize);
    Task DeleteByIdAsync(int id);
}
