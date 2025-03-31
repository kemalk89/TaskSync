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

    public string GetProjectManagerId()
    {
        var result =  ProjectMembers.FirstOrDefault(m => m.Role == "ProjectManager");
        return result != null ? result.UserId : string.Empty;
    }
    
    public Project ToDomainObject(
        User? createdBy = null, 
        IDictionary<string, User>? projectManagerMap = null)
    {
        User? manager = null;
        if (!string.IsNullOrWhiteSpace(GetProjectManagerId()) && projectManagerMap != null)
        {
            projectManagerMap.TryGetValue(GetProjectManagerId(), out manager);
        }
        
        var result = new Project
        {
            Id = Id,
            Title = Title,
            Description = Description,
            Visibility = Visibility,
            CreatedBy = createdBy,
            CreatedDate = CreatedDate,
            ModifiedDate = ModifiedDate
        };

        if (manager != null)
        {
            result.ProjectMembers.Add(new ProjectMember { User = manager, Role = "ProjectManager"});
        }
        
        return result;
    }
}