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
    public DbSet<SprintEntity> Sprints { get; set; }
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHostEnvironment _env;


    public DatabaseContext(DbContextOptions<DatabaseContext> options, IHttpContextAccessor httpContextAccessor, IHostEnvironment env)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        _env = env;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.Property<byte[]>("HashedPassword")
                .HasColumnType("bytea");

            entity.Property<byte[]>("Salt")
                .HasColumnType("bytea");
        });
        
        modelBuilder.Entity<ProjectEntity>()
            .HasMany<SprintEntity>()
            .WithOne()
            .HasForeignKey(e => e.ProjectId)
            .IsRequired();

        modelBuilder.Entity<SprintEntity>()
            .HasMany<TicketEntity>()
            .WithOne()
            .HasForeignKey(e => e.SprintId);
        
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
            CreatedBy = 1,
            CreatedDate = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero)
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
        
        CreateDemoTickets(modelBuilder, projectId: 1);
    }

    private void CreateDemoUsers(ModelBuilder modelBuilder)
    {
        var createdDate = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        
        modelBuilder.Entity<UserEntity>().HasData(new UserEntity { Id = 1, Email = "Kerem.Karacay@tasksync.test", Username = "Kerem Karacay", Picture = "", CreatedDate = createdDate });
        modelBuilder.Entity<UserEntity>().HasData(new UserEntity { Id = 2, Email = "Deniz.Aslansu@tasksync.test", Username = "Deniz Aslansu", Picture = "", CreatedDate = createdDate });
        modelBuilder.Entity<UserEntity>().HasData(new UserEntity { Id = 3, Email = "Ali.Balci@tasksync.test", Username = "Ali Balcı", Picture = "", CreatedDate = createdDate });
        modelBuilder.Entity<UserEntity>().HasData(new UserEntity { Id = 4, Email = "Sven.Imker@tasksync.test", Username = "Sven Imker", Picture = "", CreatedDate = createdDate });
        modelBuilder.Entity<UserEntity>().HasData(new UserEntity { Id = 5, Email = "Mina.Koch@tasksync.test", Username = "Mina Koch", Picture = "", CreatedDate = createdDate });
        if (_env.IsDevelopment())
        {
            modelBuilder.Entity<UserEntity>().HasData(new UserEntity { Id = 6, Email = "IntegrationTests.User1@tasksync.test", Username = "Test User1", Picture = "", ExternalUserId = "integration_tests|01", CreatedDate = createdDate });
        }
    }
    
    private void CreateDemoTickets(ModelBuilder modelBuilder, int projectId)
    {
        modelBuilder.Entity<TicketEntity>().HasData(new TicketEntity
        {
            Id = 1,
            Type = TicketType.Bug,
            Title = "Profilbild wird nach dem Upload nicht aktualisiert",
            Description = GetDescription("Wenn der Benutzer ein Profilbild hochgeladen hat, wird weiterhin das alte angezeigt."),
            CreatedBy = 0,
            ProjectId = projectId,
            StatusId = 1,
            CreatedDate = new DateTimeOffset(2026, 3, 9, 9, 17, 0, TimeSpan.Zero)
        });
        
        modelBuilder.Entity<TicketEntity>().HasData(new TicketEntity
        {
            Id = 2,
            Type = TicketType.Task,
            Title = "Secrets rotieren",
            Description = GetDescription("Die Secrets laufen Ende Februar aus."),
            CreatedBy = 0,
            ProjectId = projectId,
            StatusId = 1,
            CreatedDate = new DateTimeOffset(2026, 2, 18, 10, 5, 0, TimeSpan.Zero)
        });
        
        modelBuilder.Entity<TicketEntity>().HasData(new TicketEntity
        {
            Id = 3,
            Type = TicketType.Story,
            Title = "Teammitglieder in Kommentaren erwähnen",
            Description = GetDescription(""),
            CreatedBy = 0,
            ProjectId = projectId,
            StatusId = 1,
            CreatedDate = new DateTimeOffset(2026, 3, 10, 14, 15, 0, TimeSpan.Zero)
        });
        
        modelBuilder.Entity<TicketEntity>().HasData(new TicketEntity
        {
            Id = 4,
            Type = TicketType.Story,
            Title = "Signup Form",
            Description = GetDescription("Required fields: username and password. API-KEY Protection des /signup endpoints."),
            CreatedBy = 0,
            ProjectId = projectId,
            StatusId = 1,
            CreatedDate = new DateTimeOffset(2026, 3, 10, 8, 15, 0, TimeSpan.Zero)
        });
    }

    /**
     * Not executed during migrations.
     * Called only during normal application runtime.
     */
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
