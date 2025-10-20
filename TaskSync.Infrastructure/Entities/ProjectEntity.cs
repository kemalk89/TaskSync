using TaskSync.Domain.Project;
using TaskSync.Domain.User;

namespace TaskSync.Infrastructure.Entities;

public class ProjectEntity : AuditedEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public ProjectVisibility? Visibility { get; set; }
    public ICollection<ProjectMemberEntity> ProjectMembers { get; set; } = new List<ProjectMemberEntity>();

    public int? GetProjectManagerId()
    {
        var result =  ProjectMembers.FirstOrDefault(m => m.Role == "ProjectManager");
        return result?.UserId;
    }
    
    public IEnumerable<int> GetProjectMemberIds()
    {
        return ProjectMembers.Select(m => m.UserId);
    }   
    
    public ProjectModel ToDomainObject(
        User? createdBy = null, 
        IDictionary<int, User>? memberMap = null)
    {
        var result = new ProjectModel
        {
            Id = Id,
            Title = Title,
            Description = Description,
            ProjectMembers = ProjectMembers.Select(m => new ProjectMember
            {
                UserId = m.UserId, 
                Role = m.Role, 
                User = memberMap != null && memberMap.TryGetValue(m.UserId, out User? value) ? value : null
            }).ToList(),
            Visibility = Visibility,
            CreatedBy = createdBy,
            CreatedDate = CreatedDate,
            ModifiedDate = ModifiedDate
        };

        return result;
    }
}