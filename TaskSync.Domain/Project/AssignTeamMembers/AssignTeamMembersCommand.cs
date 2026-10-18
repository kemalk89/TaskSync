namespace TaskSync.Domain.Project.AssignTeamMembers;

public class AssignTeamMembersCommand
{
    public ICollection<TeamMember> TeamMembers { get; set; } = [];
}

public record TeamMember(string UserId, string Role);


