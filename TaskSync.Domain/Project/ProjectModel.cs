namespace TaskSync.Domain.Project;

public class ProjectModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ProjectVisibilityModel? Visibility { get; set; }

    public ICollection<ProjectMemberModel> ProjectMembers = [];
    public User.User? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }

}
