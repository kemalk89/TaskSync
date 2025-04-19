using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using TaskSync.Domain.Ticket;
using TaskSync.Infrastructure.Entities;

namespace TaskSync.Infrastructure;

public class DatabaseContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
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
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        // setup project member to user relation
        modelBuilder.Entity<UserEntity>()
            .HasMany<ProjectMemberEntity>()
            .WithOne()
            .HasForeignKey(e => e.UserId)
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
        CreateDemoUsers(modelBuilder);
        
        modelBuilder.Entity<TicketStatusEntity>().HasData(new TicketStatusEntity { Id = 1, Name = "Todo" });
        modelBuilder.Entity<TicketStatusEntity>().HasData(new TicketStatusEntity { Id = 2, Name = "In Progress" });
        modelBuilder.Entity<TicketStatusEntity>().HasData(new TicketStatusEntity { Id = 3, Name = "Done" });

        // demo project #1
        modelBuilder.Entity<ProjectEntity>().HasData(new ProjectEntity
        {
            Id = 1,
            Title = "My First Project",
            Description = "This is the description of the first project. This project has one member as well.",
            CreatedBy = 1
        });
        
        modelBuilder.Entity<ProjectMemberEntity>().HasData(new ProjectMemberEntity
        {
            Id = 1, ProjectId = 1, UserId = 1, Role = "Software Developer"
        });
        
        CreateDemoTickets(modelBuilder, projectId: 1, amount: 127);

        // demo project #2
        modelBuilder.Entity<ProjectEntity>().HasData(new ProjectEntity
        {
            Id = 2,
            Title = "My 2nd Project",
            Description = "This is the description of the 2nd project. This project has two members.",
            CreatedBy = 2
        });

        modelBuilder.Entity<ProjectMemberEntity>().HasData(new ProjectMemberEntity
        {
            Id = 2, ProjectId = 2, UserId = 1, Role = "ProjectManager"
        });
        
        modelBuilder.Entity<ProjectMemberEntity>().HasData(new ProjectMemberEntity
        {
           Id = 3, ProjectId = 2, UserId = 2, Role = "UI / UX"
        });

        CreateDemoTickets(modelBuilder, projectId: 2, amount: 234);

        // demo project #3
        modelBuilder.Entity<ProjectEntity>().HasData(new ProjectEntity
        {
            Id = 3,
            Title = "My 3rd Project",
            Description = "This is the description of the 3rd project.",
            CreatedBy = 0
        });

        CreateDemoTickets(modelBuilder, projectId: 3, amount: 3);

        // demo project #4 - has no tickets
        modelBuilder.Entity<ProjectEntity>().HasData(new ProjectEntity
        {
            Id = 4,
            Title = "My 4rd Project",
            Description = "This is the description of the 4rd project.",
            CreatedBy = 0
        });
    }

    private static TicketType GetRandomTicketType()
    {
        var values = Enum.GetValues(typeof(TicketType));
        var randomType = values.GetValue(new Random().Next(values.Length));
        return randomType != null ? (TicketType) randomType : TicketType.Task;
    }

    private void CreateDemoUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>().HasData(new UserEntity { Id = 1, Email = "empty", Username = "Kerem Karacay", Picture = "" });
        modelBuilder.Entity<UserEntity>().HasData(new UserEntity { Id = 2, Email = "empty", Username = "Deniz Aslansu", Picture = "" });
    }
    
    private void CreateDemoTickets(ModelBuilder modelBuilder, int projectId, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            TicketIdTracker++;

            var title = "Not set";
            var ticketType = GetRandomTicketType();
            if (ticketType == TicketType.Task)
            {
                title = $"Demo Ticket of type Task #{TicketIdTracker}";
            } else if (ticketType == TicketType.Story)
            {
                title = $"Demo Ticket of type Story #{TicketIdTracker}";
            } else if (ticketType == TicketType.Bug)
            {
                title = $"Demo Ticket of type Bug #{TicketIdTracker}";
            }
            
            modelBuilder.Entity<TicketEntity>().HasData(new TicketEntity
            {
                Id = TicketIdTracker,
                Type = ticketType,
                Title = title,
                Description = $"This is the description of the demo ticket #{TicketIdTracker}.",
                CreatedBy = 0,
                ProjectId = projectId,
                StatusId = new Random().Next(1, 4) // Generates a random number between 1 (inclusive) and 4 (exclusive)
            });
        }
    }

    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
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

                var currentUserId = 0;
                var currentUser = await Users.FirstOrDefaultAsync(u => u.ExternalUserId == authenticatedUserId);
                if (currentUser == null)
                {
                    // Throw an exception unless we're inserting a new User record - because in this case the user is not yet available in the users table!
                    if (audited.GetType().Name != nameof(UserEntity))
                    {
                        throw new Exception($"No user found for external user id {authenticatedUserId}");
                    }
                }
                else
                {
                    currentUserId = currentUser.Id;
                }

                audited.CreatedBy = currentUserId;
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

        return await base.SaveChangesAsync(cancellationToken);
    }
}
