using TaskSync.Domain.Shared;
using Microsoft.Extensions.Logging;

using TaskSync.Domain.User;

namespace TaskSync.Domain.Project.UpdateProject;

public class UpdateProjectCommandHandler : ICommandHandler
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<UpdateProjectCommandHandler> _logger;

    public UpdateProjectCommandHandler(
        IProjectRepository projectRepository, 
        ILogger<UpdateProjectCommandHandler> logger, 
        ICurrentUserService currentUserService)
    {
        _projectRepository = projectRepository;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<Result<bool>> HandleAsync(int projectId, UpdateProjectCommand updateProjectCommand)
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
    
    private ProjectMemberModel? FindProjectManager(ProjectModel projectModel)
    {
        return projectModel.ProjectMembers.FirstOrDefault((p) => p.Role == "ProjectManager");
    }
}