using Microsoft.EntityFrameworkCore;

using TaskSync.Domain.Project;
using TaskSync.Domain.Project.CreateProject;
using TaskSync.Domain.Project.UpdateProject;
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

    public async Task<ProjectModel> CreateAsync(CreateProjectCommand command)
    {
        var entity = new ProjectEntity
        {
            Title = command.Title,
            Description = command.Description,
            Visibility = command.Visibility,
        };

        if (command.ProjectManagerId != null)
        {
            var projectManager = new ProjectMemberEntity { UserId = (int)command.ProjectManagerId, Role = "ProjectManager" };
            entity.ProjectMembers.Add(projectManager);
        }
        
        await _dbContext.Projects.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return new ProjectModel
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description
        };
    }

    public async Task<PagedResult<ProjectModel>> GetAllAsync(int pageNumber, int pageSize)
    {
        var skip = (pageNumber - 1) * pageSize;

        var records = _dbContext.Projects
            .Include(p => p.ProjectMembers)
            .OrderBy(item => item.CreatedDate)
            .Skip(skip)
            .Take(pageSize)
            .ToList();

        // fetch project members
        var projectMemberIds = records
            .SelectMany(project => project.ProjectMembers)
            .Select(projectMember => projectMember.UserId)
            .Distinct()
            .ToArray();

        var projectMembers = await this._userRepository.FindUsersAsync(projectMemberIds.Distinct().ToArray());
        
        var projectMemberMap = projectMembers.ToDictionary(u => u.Id, u => u);
        var projects = records.Select(item => item.ToDomainObject(null, projectMemberMap));
        
        int total = _dbContext.Projects.Count();

        var paged = new PagedResult<ProjectModel>
        {
            Items = projects,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Total = total
        };

        return paged;
    }

    public async Task<ProjectModel?> GetByIdAsync(int id)
    {
        ProjectEntity? entity = await _dbContext.Projects
            .Include(p => p.ProjectMembers)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (entity == null)
        {
            return null;
        }

        var memberIds = entity.GetProjectMemberIds().ToArray();
        var projectMembers = await _userRepository.FindUsersAsync(memberIds);
        var projectManagerMap = projectMembers.ToList().ToDictionary(u => u.Id, u => u);
        
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

    public async Task SaveAsync(ProjectModel projectModel)
    {
        var record = await _dbContext.Projects.FindAsync(projectModel.Id);
        if (record == null)
        {
            // TODO Implement 
            throw new NotImplementedException();
        }
        
        var projectMembers = projectModel.ProjectMembers
            .Select(m => new ProjectMemberEntity { UserId = m.UserId, Role = m.Role, ProjectId = record.Id });
        
        record.ProjectMembers = projectMembers.ToList();

        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateProjectAsync(int projectId, UpdateProjectCommand command)
    {
        var project = await _dbContext.Projects.FindAsync(projectId);
        if (project == null)
        {
            throw new KeyNotFoundException($"Project with ID {projectId} not found.");
        }

        if (command.ProjectManagerId != null)
        {
            project.ProjectMembers.Add(new ProjectMemberEntity
            {
                Role = "ProjectManager", 
                UserId = (int) command.ProjectManagerId, 
                ProjectId = projectId
            });
        }

        if (!string.IsNullOrWhiteSpace(command.Title))
        {
            project.Title = command.Title;
        }

        if (command.Description != null)
        {
            project.Description = command.Description;
        }
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task<int> CreateLabelAsync(int projectId, string text)
    {
        var entity = new TicketLabelEntity { ProjectId = projectId, Text = text };
        var result = await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return result.Entity.Id;
    }

    public async Task<List<ProjectLabelModel>> GetLabelsAsync(int projectId)
    {
        return await _dbContext.TicketLabels
            .Where(e => e.ProjectId == projectId)
            .Select(e => new ProjectLabelModel
            {
                Id = e.Id,
                Text = e.Text
            })
            .ToListAsync();
    }
}
