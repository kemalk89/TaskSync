using Microsoft.Extensions.Logging;

using TaskSync.Domain.Project.Commands;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket;
using TaskSync.Domain.User;

using ResultCodes = TaskSync.Domain.Shared.ResultCodes;

namespace TaskSync.Domain.Project;

public class ProjectService : IProjectService
{
    private readonly ILogger<ProjectService> _logger;
    
    private readonly IProjectRepository _projectRepository;
    private readonly ITicketService _ticketService;
    private readonly ICurrentUserService _currentUserService;
    
    public ProjectService(
        IProjectRepository projectRepository,
        ITicketService ticketService, 
        ICurrentUserService currentUserService, 
        ILogger<ProjectService> logger)
    {
        _projectRepository = projectRepository;
        _ticketService = ticketService;
        _currentUserService = currentUserService;
        _logger = logger;
    }

    public async Task<Project> CreateProjectAsync(CreateProjectCommand command)
    {
        var project = await _projectRepository.CreateAsync(command);
        return project;
    }

    public async Task<Project?> GetProjectByIdAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        return project;
    }

    public async Task<PagedResult<Project>> GetProjectsAsync(int pageNumber, int pageSize)
    {
        var projects = await _projectRepository.GetAllAsync(pageNumber, pageSize);
        return projects;
    }

    public async Task DeleteProjectAsync(int id)
    {
        await _projectRepository.DeleteByIdAsync(id);
    }

    public async Task<PagedResult<TicketModel>> GetProjectTicketsAsync(int projectId, int pageNumber = 1, int pageSize = 0)
    {
        var project = await GetProjectByIdAsync(projectId);
        if (project == null)
        {
            return new PagedResult<TicketModel>
            {
                Total = 0,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        return await _ticketService.GetTicketsByProjectIdAsync(projectId, pageNumber, pageSize);
    }
    
    public async Task<Result<bool>> AssignTeamMembersAsync(int projectId, AssignTeamMembersCommand command)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
        {
            return Result<bool>.Fail("Project not found. ID " + projectId);
        }
        
        command.TeamMembers.ToList().ForEach(m => project.ProjectMembers.Add(new ProjectMember
        {
            UserId = m.UserId,
            Role = m.Role,
        }));

        await _projectRepository.SaveAsync(project);
        return Result<bool>.Ok(true);
    }

    public async Task<Result<bool>> UpdateProjectAsync(int projectId, UpdateProjectCommand updateProjectCommand)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
        {
            _logger.LogInformation("Project not found. ID {ProjectId}", projectId);
            return Result<bool>.Fail(ResultCodes.ResultCodeResourceNotFound);
        }
        
        var currentUser = await _currentUserService.GetCurrentUserAsync();
        if (project.CreatedBy != null && project.CreatedBy.Id != currentUser?.Id)
        {
            _logger.LogInformation("No permissions: Current user cannot assign project manager to project with ID {ProjectId}", projectId);
            return Result<bool>.Fail(ResultCodes.ResultCodeNoPermissions);
        }
        
        if (updateProjectCommand.ProjectManagerId != null)
        {
            var currentProjectManager = FindProjectManager(project);
            if (currentProjectManager != null)
            {
                _logger.LogInformation("Project (ID: {ProjectId}) already have a project manager assigned (ID: {UserId}). Going to replace with new project manager (ID: {UserId1}).",
                    project.Id, currentProjectManager.UserId, updateProjectCommand.ProjectManagerId);   
            }
        }

        await _projectRepository.UpdateProjectAsync(projectId, updateProjectCommand);
        
        return Result<bool>.Ok(true);
    }

    private ProjectMember? FindProjectManager(Project project)
    {
        return project.ProjectMembers.FirstOrDefault((p) => p.Role == "ProjectManager");
    }
}

