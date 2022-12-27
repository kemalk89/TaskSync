using Microsoft.EntityFrameworkCore;
using Times.Infrastructure.Entities;

namespace Times.Infrastructure;

public class DatabaseContext : DbContext
{
    public DbSet<TaskEntity> Tasks { get; set; }
    public DbSet<ProjectEntity> Projects { get; set; }
}
