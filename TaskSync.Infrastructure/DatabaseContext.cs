using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TaskSync.Infrastructure.Entities;

namespace TaskSync.Infrastructure;

public class DatabaseContext : DbContext
{
    public DbSet<TicketEntity> Tickets { get; set; }
    public DbSet<TicketCommentEntity> TicketComments { get; set; }
    public DbSet<TicketStatusEntity> TicketStatus { get; set; }
    public DbSet<ProjectEntity> Projects { get; set; }
    private readonly IHttpContextAccessor _httpContextAccessor;

    private int TicketIdTracker = 0;

    public DatabaseContext(DbContextOptions<DatabaseContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProjectMemberEntity>()
            .HasOne<ProjectEntity>() 
            .WithMany(p => p.ProjectMembers) 
            .HasForeignKey(pm => pm.ProjectId)
            .IsRequired(false);
        
        modelBuilder.Entity<TicketEntity>()
            .HasOne(t => t.Project)
            .WithMany()
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TicketCommentEntity>()
            .HasOne(entity => entity.Ticket)
            .WithMany()
            .HasForeignKey(entity => entity.TicketId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // seed data: https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding
        modelBuilder.Entity<TicketStatusEntity>().HasData(new TicketStatusEntity { Id = 1, Name = "Todo" });
        modelBuilder.Entity<TicketStatusEntity>().HasData(new TicketStatusEntity { Id = 2, Name = "In Progress" });
        modelBuilder.Entity<TicketStatusEntity>().HasData(new TicketStatusEntity { Id = 3, Name = "Done" });

        // demo project #1
        modelBuilder.Entity<ProjectEntity>().HasData(new ProjectEntity
        {
            Id = 1,
            Title = "My First Project",
            Description = "This is the description of the first project.",
            CreatedBy = ""
        });

        CreateDemoTickets(modelBuilder, projectId: 1, amount: 127);

        // demo project #2
        modelBuilder.Entity<ProjectEntity>().HasData(new ProjectEntity
        {
            Id = 2,
            Title = "My 2nd Project",
            Description = "This is the description of the 2nd project.",
            CreatedBy = ""
        });

        CreateDemoTickets(modelBuilder, projectId: 2, amount: 234);

        // demo project #3
        modelBuilder.Entity<ProjectEntity>().HasData(new ProjectEntity
        {
            Id = 3,
            Title = "My 3rd Project",
            Description = "This is the description of the 3rd project.",
            CreatedBy = ""
        });

        CreateDemoTickets(modelBuilder, projectId: 3, amount: 3);

        // demo project #4 - has no tickets
        modelBuilder.Entity<ProjectEntity>().HasData(new ProjectEntity
        {
            Id = 4,
            Title = "My 4rd Project",
            Description = "This is the description of the 4rd project.",
            CreatedBy = ""
        });
    }

    private void CreateDemoTickets(ModelBuilder modelBuilder, int projectId, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            TicketIdTracker++;

            modelBuilder.Entity<TicketEntity>().HasData(new TicketEntity
            {
                Id = TicketIdTracker,
                Title = $"Demo Ticket #{TicketIdTracker}",
                Description = $"This is the description of the demo ticket #{TicketIdTracker}.",
                CreatedBy = "",
                ProjectId = projectId,
                StatusId = new Random().Next(1, 4) // Generates a random number between 1 (inclusive) and 4 (exclusive)
            });
        }
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
            if (e is AuditedEntity audited)
            {
                audited.ModifiedDate = DateTimeOffset.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
