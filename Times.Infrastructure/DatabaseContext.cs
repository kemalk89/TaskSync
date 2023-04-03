using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Times.Infrastructure.Entities;

namespace Times.Infrastructure;

public class DatabaseContext : DbContext
{
    public DbSet<TicketEntity> Tickets { get; set; }
    public DbSet<TicketStatusEntity> TicketStatus { get; set; }
    public DbSet<ProjectEntity> Projects { get; set; }
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DatabaseContext(DbContextOptions<DatabaseContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TicketEntity>()
            .HasOne(t => t.Project)
            .WithMany()
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // seed data: https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding
        modelBuilder.Entity<TicketStatusEntity>().HasData(new TicketStatusEntity { Id = 1, Name = "Todo" });
        modelBuilder.Entity<TicketStatusEntity>().HasData(new TicketStatusEntity { Id = 2, Name = "In Progress" });
        modelBuilder.Entity<TicketStatusEntity>().HasData(new TicketStatusEntity { Id = 3, Name = "Done" });

        modelBuilder.Entity<ProjectEntity>().HasData(new ProjectEntity
        {
            Id = 1,
            Title = "My First Project",
            Description = "This is the description of the first project.",
            CreatedBy = ""
        });

        modelBuilder.Entity<TicketEntity>().HasData(new TicketEntity
        {
            Id = 1,
            Title = "My First Ticket",
            Description = "This is the description of the first ticket.",
            CreatedBy = "",
            ProjectId = 1,
            StatusId = 1
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var inserted = this.ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Added)
            .Select(x => x.Entity);

        var modified = this.ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Modified)
            .Select(x => x.Entity);

        foreach (var e in inserted)
        {
            var audited = e as AuditedEntity;
            if (audited != null)
            {
                var authenticatedUserId = _httpContextAccessor.HttpContext.User.Identity?.Name;
                if (authenticatedUserId == null)
                {
                    throw new Exception("No authenticated user");
                }
                audited.CreatedBy = authenticatedUserId;
                audited.CreatedDate = DateTimeOffset.UtcNow;
            }
        }

        foreach (var e in modified)
        {
            var audited = e as AuditedEntity;
            if (audited != null)
            {
                audited.ModifiedDate = DateTimeOffset.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
