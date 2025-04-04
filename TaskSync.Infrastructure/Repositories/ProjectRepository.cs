using Microsoft.EntityFrameworkCore;

using TaskSync.Domain.Project;
using TaskSync.Domain.Project.Commands;
using TaskSync.Domain.Shared;
using TaskSync.Domain.User;
using TaskSync.Infrastructure.Entities;

namespace TaskSync.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{

    private readonly DatabaseContext _dbContext;
    private readonly IUserRepository _userRepository;

    public ProjectRepository(DatabaseContext dbContext, IUserRepository userRepository)
    {
        _dbContext = dbContext;
        _userRepository = userRepository;
    }

    public async Task<Project> CreateAsync(CreateProjectCommand command)
    {
        var entity = new ProjectEntity
        {
            Title = command.Title,
            Description = command.Description,
            Visibility = command.Visibility,
        };

        if (!string.IsNullOrWhiteSpace(command.ProjectManagerId))
        {
            var projectManager = new ProjectMemberEntity { UserId = command.ProjectManagerId, Role = "ProjectManager" };
            entity.ProjectMembers.Add(projectManager);
        }
        
        await _dbContext.Projects.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return new Project
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description
        };
    }

    public async Task<PagedResult<Project>> GetAllAsync(int pageNumber, int pageSize)
    {
        var skip = (pageNumber - 1) * pageSize;

        var records = _dbContext.Projects
            .OrderBy(item => item.CreatedDate)
            .Skip(skip)
            .Take(pageSize)
            .ToList();

        // fetch project managers
        var projectManagerIds = records
            .Where(r => !string.IsNullOrWhiteSpace(r.GetProjectManagerId()))
            .Select(r => r.GetProjectManagerId())
            .Distinct()
            .ToArray();
        var projectManagers = (await this._userRepository.FindUsersAsync(projectManagerIds)).ToDictionary(u => u.Id, u => u);
        var projects = records.Select(item => item.ToDomainObject(null, projectManagers));
        
        int total = _dbContext.Projects.Count();

        var paged = new PagedResult<Project>
        {
            Items = projects,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Total = total
        };

        return paged;
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        ProjectEntity? entity = await _dbContext.Projects
            .Include(p => p.ProjectMembers)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (entity == null)
        {
            return null;
        }

        var projectManagerMap = new Dictionary<string, User>();
        var projectMembers = await _userRepository.FindUsersAsync(entity.GetProjectMemberIds().ToArray());
        projectMembers.ToList().ForEach(m => projectManagerMap.Add(m.Id, m));    
        
        var createdBy = await _userRepository.FindUserByIdAsync(entity.CreatedBy);
        return entity.ToDomainObject(createdBy, projectManagerMap);
    }

    public async Task DeleteByIdAsync(int id)
    {

        var entity = new ProjectEntity { Id = id };
        _dbContext.Attach(entity);
        _dbContext.Projects.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task SaveAsync(Project project)
    {
        var record = await _dbContext.Projects.FindAsync(project.Id);
        if (record == null)
        {
            // TODO Implement 
            throw new NotImplementedException();
        }
        
        var projectMembers = project.ProjectMembers
            .Select(m => new ProjectMemberEntity { UserId = m.UserId, Role = m.Role, ProjectId = record.Id });
        
        record.ProjectMembers = projectMembers.ToList();

        await _dbContext.SaveChangesAsync();
    }
}
