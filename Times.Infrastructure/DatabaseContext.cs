using Microsoft.EntityFrameworkCore;
using Times.Infrastructure.Entities;

namespace Times.Infrastructure;

public class DatabaseContext : DbContext
{
    public DbSet<TaskEntity> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connString = "Host=localhost;Port=5433;Database=times;Username=postgres;Password=example";
        options.UseNpgsql(connString);
    }
}
