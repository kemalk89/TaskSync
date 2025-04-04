namespace TaskSync.Infrastructure.Entities;

public class ProjectMemberEntity
{
    public int Id { get; set; }
    
    public string Role { get; set; } = "";
    public string UserId { get; set; } = "";
    
    // Optional foreign key property
    public int ProjectId { get; set; }
}