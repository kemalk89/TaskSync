using System.ComponentModel.DataAnnotations.Schema;

using TaskSync.Domain.User;

namespace TaskSync.Infrastructure.Entities;

public class ProjectMemberEntity
{
    public int Id { get; set; }
    
    public string Role { get; set; } = "";
    public int UserId { get; set; }
    [NotMapped]
    public User User { get; set; }
    
    // Optional foreign key property
    public int ProjectId { get; set; }
}