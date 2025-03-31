using TaskSync.Domain.Project.Commands;
using TaskSync.Domain.Shared;

namespace TaskSync.Domain.Project;

public interface IProjectRepository
{
    Task<Project> CreateAsync(CreateProjectCommand command);
    Task<Project?> GetByIdAsync(int id);
    Task<PagedResult<Project>> GetAllAsync(int pageNumber, int pageSize);
    Task DeleteByIdAsync(int id);
}
