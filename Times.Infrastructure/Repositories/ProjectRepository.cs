using Times.Domain.Project;
using Times.Infrastructure.Entities;

namespace Times.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{

    private readonly DatabaseContext _dbContext;

    public ProjectRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Project> CreateAsync(string title, string? description)
    {
        var entity = new ProjectEntity
        {
            Title = title,
            Description = description
        };

        await _dbContext.Projects.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return new Project
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description
        };
    }

    public Task<IEnumerable<Project>> GetAllAsync()
    {
        var projects = _dbContext.Projects
            .ToList()
            .Select(item => item.ToProject());

        return Task.FromResult(projects);
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        ProjectEntity? entity = await _dbContext.Projects.FindAsync(id);

        if (entity == null)
        {
            return null;
        }

        return entity.ToProject();
    }
}
