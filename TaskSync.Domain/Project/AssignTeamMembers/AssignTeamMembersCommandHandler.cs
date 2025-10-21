using TaskSync.Domain.Shared;

namespace TaskSync.Domain.Project.AssignTeamMembers;

public class AssignTeamMembersCommandHandler : ICommandHandler
{
    private readonly IProjectRepository _projectRepository;

    public AssignTeamMembersCommandHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Result<bool>> HandleAsync(int projectId, AssignTeamMembersCommand command)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
        {
            return Result<bool>.Fail("Project not found. ID " + projectId);
        }
        
        command.TeamMembers.ToList().ForEach(m => project.ProjectMembers.Add(new ProjectMemberModel
        {
            UserId = m.UserId,
            Role = m.Role,
        }));

        await _projectRepository.SaveAsync(project);
        return Result<bool>.Ok(true);
    }
    
}