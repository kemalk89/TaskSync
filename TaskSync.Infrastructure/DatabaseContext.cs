using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

using TaskSync.Domain.Ticket;
using TaskSync.Infrastructure.Entities;

namespace TaskSync.Infrastructure;

public class DatabaseContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<TicketEntity> Tickets { get; set; }
    public DbSet<TicketCommentEntity> TicketComments { get; set; }
    public DbSet<TicketStatusEntity> TicketStatus { get; set; }
    public DbSet<TicketLabelEntity> TicketLabels { get; set; }
    
    public DbSet<ProjectEntity> Projects { get; set; }
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHostEnvironment _env;

    private int _ticketIdTracker;

    public DatabaseContext(DbContextOptions<DatabaseContext> options, IHttpContextAccessor httpContextAccessor, IHostEnvironment env)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        _env = env;
        _ticketIdTracker = 0;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TicketEntity>()
            .HasMany(t => t.Labels)
            .WithMany()
            .UsingEntity("TicketLabelMapping"); // assign custom name to mapping table
        
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
        
        modelBuilder.Entity<UserEntity>()
            .HasIndex(u => u.Email)
            .IsUnique();
        
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
        
        modelBuilder.Entity<TicketLabelEntity>()
            .HasOne<ProjectEntity>()
            .WithMany()
            .HasForeignKey(e => e.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // seed data: https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding
        CreateDemoUsers(modelBuilder);
        
        modelBuilder.Entity<TicketStatusEntity>().HasData(new TicketStatusEntity { Id = 1, Name = "Todo" });
        modelBuilder.Entity<TicketStatusEntity>().HasData(new TicketStatusEntity { Id = 2, Name = "In Progress" });
        modelBuilder.Entity<TicketStatusEntity>().HasData(new TicketStatusEntity { Id = 3, Name = "Done" });
        
        // demo project
        modelBuilder.Entity<ProjectEntity>().HasData(new ProjectEntity
        {
            Id = 1,
            Title = "My First Project",
            Description = GetDescription("This is the description of the first project. This project has one member as well."),
            CreatedBy = 1
        });
        
        // demo project has 3 labels
        modelBuilder.Entity<TicketLabelEntity>().HasData(new TicketLabelEntity { Id = 1, Text = "Need's Design", ProjectId = 1 });
        modelBuilder.Entity<TicketLabelEntity>().HasData(new TicketLabelEntity { Id = 2, Text = "Need's Discussion", ProjectId = 1 });
        modelBuilder.Entity<TicketLabelEntity>().HasData(new TicketLabelEntity { Id = 3, Text = "Quick Fix", ProjectId = 1 });
        
        // demo project has 2 team members
        modelBuilder.Entity<ProjectMemberEntity>().HasData(new ProjectMemberEntity
        {
            Id = 1, ProjectId = 1, UserId = 1, Role = "Software Developer"
        });

        modelBuilder.Entity<ProjectMemberEntity>().HasData(new ProjectMemberEntity
        {
            Id = 2, ProjectId = 1, UserId = 2, Role = "UI / UX"
        });
        
        CreateDemoTickets(modelBuilder, projectId: 1, amount: 12);
    }

    private static TicketType GetRandomTicketType()
    {
        var values = Enum.GetValues(typeof(TicketType));
        var randomType = values.GetValue(new Random().Next(values.Length));
        return randomType != null ? (TicketType) randomType : TicketType.Task;
    }

    private void CreateDemoUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>().HasData(new UserEntity { Id = 1, Email = "Kerem.Karacay@tasksync.test", Username = "Kerem Karacay", Picture = "", CreatedDate = DateTimeOffset.UtcNow });
        modelBuilder.Entity<UserEntity>().HasData(new UserEntity { Id = 2, Email = "Deniz.Aslansu@tasksync.test", Username = "Deniz Aslansu", Picture = "", CreatedDate = DateTimeOffset.UtcNow });
        modelBuilder.Entity<UserEntity>().HasData(new UserEntity { Id = 3, Email = "Ali.Balci@tasksync.test", Username = "Ali Balcı", Picture = "", CreatedDate = DateTimeOffset.UtcNow });
        modelBuilder.Entity<UserEntity>().HasData(new UserEntity { Id = 4, Email = "Sven.Imker@tasksync.test", Username = "Sven Imker", Picture = "", CreatedDate = DateTimeOffset.UtcNow });
        modelBuilder.Entity<UserEntity>().HasData(new UserEntity { Id = 5, Email = "Mina.Koch@tasksync.test", Username = "Mina Koch", Picture = "", CreatedDate = DateTimeOffset.UtcNow });
        if (_env.IsDevelopment())
        {
            modelBuilder.Entity<UserEntity>().HasData(new UserEntity { Id = 6, Email = "IntegrationTests.User1@tasksync.test", Username = "Test User1", Picture = "", ExternalUserId = "integration_tests|01", CreatedDate = DateTimeOffset.UtcNow });
        }
    }
    
    private void CreateDemoTickets(ModelBuilder modelBuilder, int projectId, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            _ticketIdTracker++;

            var title = "Not set";
            var ticketType = GetRandomTicketType();
            if (ticketType == TicketType.Task)
            {
                title = $"Demo Ticket of type Task #{_ticketIdTracker}";
            } else if (ticketType == TicketType.Story)
            {
                title = $"Demo Ticket of type Story #{_ticketIdTracker}";
            } else if (ticketType == TicketType.Bug)
            {
                title = $"Demo Ticket of type Bug #{_ticketIdTracker}";
            }
            
            modelBuilder.Entity<TicketEntity>().HasData(new TicketEntity
            {
                Id = _ticketIdTracker,
                Type = ticketType,
                Title = title,
                Description = GetDescription($"This is the description of the demo ticket #{_ticketIdTracker}."),
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
                var authenticatedUserId = _httpContextAccessor.HttpContext?.User.Identity?.Name;
                if (authenticatedUserId == null)
                {
                    throw new Exception("No authenticated user");
                }

                var currentUserId = 0;
                var currentUser = await Users.FirstOrDefaultAsync(u => u.ExternalUserId == authenticatedUserId, cancellationToken);
                var users = await Users.ToListAsync();
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

    private string GetDescription(string text)
    {
        return $$"""
               {
                 "type": "doc",
                 "content": [
                   {
                     "type": "paragraph",
                     "content": [
                       {
                         "type": "text",
                         "text": "{{text}}"
                       }
                     ]
                   },
                   { "type": "paragraph" }
                 ]
               }
               """;
    }
}
