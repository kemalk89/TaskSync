using Times.Domain.Project;
using Times.Domain.Shared;
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

    public Task<PagedResult<Project>> GetAllAsync(int pageNumber, int pageSize)
    {
        var skip = (pageNumber - 1) * pageSize;

        var projects = _dbContext.Projects
            .OrderBy(item => item.Title)
            .Skip(skip)
            .Take(pageSize)
            .ToList()
            .Select(item => item.ToProject());

        int total = _dbContext.Projects.Count();

        var paged = new PagedResult<Project>
        {
            Items = projects,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Total = total
        };

        return Task.FromResult(paged);
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

    public async Task DeleteByIdAsync(int id)
    {

        var entity = new ProjectEntity { Id = id };
        _dbContext.Attach(entity);
        _dbContext.Projects.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}
