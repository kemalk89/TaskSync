
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

using TaskSync.Domain.Ticket;

using TaskSync.Infrastructure.Entities;

namespace TaskSync.Infrastructure;

/// <summary>
/// Reference: https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding
/// </summary>
public class DataSeeding
{
    private readonly IHostEnvironment _env;

    private int TicketIdCounter = 0;
    private const int USER_ID_KEREM = 1;
    private const int USER_ID_DENIZ = 2;
    private const int USER_ID_ALI = 3;
    private const int USER_ID_SVEN = 4;
    private const int USER_ID_MINA = 5;
    private const int STATUS_ID_TODO = 1;
    private const int STATUS_ID_IN_PROGRESS = 2;
    private const int STATUS_ID_DONE = 3;

    public DataSeeding(IHostEnvironment env)
    {
        _env = env;
    }

    public void Seed(ModelBuilder modelBuilder)
    {
        CreateDemoUsers(modelBuilder);

        modelBuilder.Entity<TicketStatusEntity>().HasData(new TicketStatusEntity { Id = STATUS_ID_TODO, Name = "Todo" });
        modelBuilder.Entity<TicketStatusEntity>().HasData(new TicketStatusEntity { Id = STATUS_ID_IN_PROGRESS, Name = "In Progress" });
        modelBuilder.Entity<TicketStatusEntity>().HasData(new TicketStatusEntity { Id = STATUS_ID_DONE, Name = "Done" });

        // demo project
        modelBuilder.Entity<ProjectEntity>().HasData(new ProjectEntity
        {
            Id = 1,
            Title = "My First Project",
            Description = GetDescription("This is the description of the first project."),
            CreatedBy = 1,
            CreatedDate = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero)
        });

        // demo project has 3 labels
        modelBuilder.Entity<TicketLabelEntity>().HasData(new TicketLabelEntity { Id = 1, Text = "Need's Design", ProjectId = 1 });
        modelBuilder.Entity<TicketLabelEntity>().HasData(new TicketLabelEntity { Id = 2, Text = "Need's Discussion", ProjectId = 1 });
        modelBuilder.Entity<TicketLabelEntity>().HasData(new TicketLabelEntity { Id = 3, Text = "Quick Fix", ProjectId = 1 });

        // demo project has 3 team members
        modelBuilder.Entity<ProjectMemberEntity>().HasData(new ProjectMemberEntity
        {
            Id = 1,
            ProjectId = 1,
            UserId = USER_ID_KEREM,
            Role = "Software Developer"
        });

        modelBuilder.Entity<ProjectMemberEntity>().HasData(new ProjectMemberEntity
        {
            Id = 2,
            ProjectId = 1,
            UserId = USER_ID_DENIZ,
            Role = "UI / UX"
        });

        modelBuilder.Entity<ProjectMemberEntity>().HasData(new ProjectMemberEntity
        {
            Id = 3,
            ProjectId = 1,
            UserId = USER_ID_ALI,
            Role = "ProjectManager"
        });

        CreateDemoTicketsInBacklog(modelBuilder, projectId: 1);

        StartSprint(modelBuilder, projectId: 1);
    }

    private void CreateDemoUsers(ModelBuilder modelBuilder)
    {
        var createdDate = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

        modelBuilder.Entity<UserEntity>().HasData(new UserEntity { Id = USER_ID_KEREM, Email = "Kerem.Karacay@tasksync.test", Username = "Kerem Karacay", Picture = "", CreatedDate = createdDate });
        modelBuilder.Entity<UserEntity>().HasData(new UserEntity { Id = USER_ID_DENIZ, Email = "Deniz.Aslansu@tasksync.test", Username = "Deniz Aslansu", Picture = "", CreatedDate = createdDate });
        modelBuilder.Entity<UserEntity>().HasData(new UserEntity { Id = USER_ID_ALI, Email = "Ali.Balci@tasksync.test", Username = "Ali Balcı", Picture = "", CreatedDate = createdDate });
        modelBuilder.Entity<UserEntity>().HasData(new UserEntity { Id = USER_ID_SVEN, Email = "Sven.Imker@tasksync.test", Username = "Sven Imker", Picture = "", CreatedDate = createdDate });
        modelBuilder.Entity<UserEntity>().HasData(new UserEntity { Id = USER_ID_MINA, Email = "Mina.Koch@tasksync.test", Username = "Mina Koch", Picture = "", CreatedDate = createdDate });
        if (_env.IsDevelopment())
        {
            modelBuilder.Entity<UserEntity>().HasData(new UserEntity { Id = 6, Email = "IntegrationTests.User1@tasksync.test", Username = "Test User1", Picture = "", ExternalUserId = "integration_tests|01", CreatedDate = createdDate });
        }
    }

    private void CreateDemoTicketsInBacklog(ModelBuilder modelBuilder, int projectId)
    {
        modelBuilder.Entity<TicketEntity>().HasData(new TicketEntity
        {
            Id = NextTicketId(),
            Type = TicketType.Bug,
            Title = "Profilbild wird nach dem Upload nicht aktualisiert",
            Description = GetDescription("Wenn der Benutzer ein Profilbild hochgeladen hat, wird weiterhin das alte angezeigt."),
            CreatedBy = 0,
            ProjectId = projectId,
            StatusId = STATUS_ID_TODO,
            CreatedDate = new DateTimeOffset(2026, 3, 9, 9, 17, 0, TimeSpan.Zero)
        });

        modelBuilder.Entity<TicketEntity>().HasData(new TicketEntity
        {
            Id = NextTicketId(),
            Type = TicketType.Task,
            Title = "Secrets rotieren",
            Description = GetDescription("Die Secrets laufen Ende Februar aus."),
            CreatedBy = 0,
            ProjectId = projectId,
            StatusId = STATUS_ID_TODO,
            CreatedDate = new DateTimeOffset(2026, 2, 18, 10, 5, 0, TimeSpan.Zero)
        });

        modelBuilder.Entity<TicketEntity>().HasData(new TicketEntity
        {
            Id = NextTicketId(),
            Type = TicketType.Story,
            Title = "Teammitglieder in Kommentaren erwähnen",
            Description = GetDescription(""),
            CreatedBy = 0,
            ProjectId = projectId,
            StatusId = STATUS_ID_TODO,
            CreatedDate = new DateTimeOffset(2026, 3, 10, 14, 15, 0, TimeSpan.Zero)
        });

        modelBuilder.Entity<TicketEntity>().HasData(new TicketEntity
        {
            Id = NextTicketId(),
            Type = TicketType.Story,
            Title = "Signup Form",
            Description = GetDescription("Required fields: username and password. API-KEY Protection des /signup endpoints."),
            CreatedBy = 0,
            ProjectId = projectId,
            StatusId = STATUS_ID_TODO,
            CreatedDate = new DateTimeOffset(2026, 3, 10, 8, 15, 0, TimeSpan.Zero)
        });
    }

    private void StartSprint(ModelBuilder modelBuilder, int projectId)
    {
        // sprint duration (2 weeks): 
        // start    09.03.2026, 10:00 (Monday)
        // end      23.03.2026, 10:00 (Monday)
        modelBuilder.Entity<SprintEntity>().HasData(new SprintEntity
        {
            Id = 1,
            ProjectId = projectId,
            StartDate = new DateTimeOffset(2026, 3, 9, 10, 0, 0, TimeSpan.Zero),
            EndDate = new DateTimeOffset(2026, 3, 23, 10, 0, 0, TimeSpan.Zero),
            CreatedDate = new DateTimeOffset(2026, 3, 9, 10, 0, 0, TimeSpan.Zero),
            CreatedBy = USER_ID_KEREM,
        });

        // assign tickets to sprint
        modelBuilder.Entity<TicketEntity>().HasData(new TicketEntity
        {
            Id = NextTicketId(),
            ProjectId = projectId,
            SprintId = 1,
            StatusId = STATUS_ID_TODO,
            Type = TicketType.Task,
            Title = "Setup Documentation",
            Description = GetDescription("Setup Documentation"),
            CreatedBy = USER_ID_ALI,
            CreatedDate = new DateTimeOffset(2026, 3, 10, 8, 15, 0, TimeSpan.Zero)
        });

        modelBuilder.Entity<TicketEntity>().HasData(new TicketEntity
        {
            Id = NextTicketId(),
            ProjectId = projectId,
            SprintId = 1,
            StatusId = STATUS_ID_DONE,
            AssigneeId = USER_ID_KEREM,
            Type = TicketType.Task,
            Title = "Setup Repository",
            Description = GetDescription("Setup Repository"),
            CreatedBy = USER_ID_ALI,
            CreatedDate = new DateTimeOffset(2026, 3, 10, 8, 15, 0, TimeSpan.Zero)
        });

        modelBuilder.Entity<TicketEntity>().HasData(new TicketEntity
        {
            Id = NextTicketId(),
            ProjectId = projectId,
            SprintId = 1,
            StatusId = STATUS_ID_IN_PROGRESS,
            AssigneeId = USER_ID_KEREM,
            Type = TicketType.Task,
            Title = "Setup Database",
            Description = GetDescription("Setup database"),
            CreatedBy = USER_ID_ALI,
            CreatedDate = new DateTimeOffset(2026, 3, 10, 8, 15, 0, TimeSpan.Zero)
        });

        modelBuilder.Entity<TicketEntity>().HasData(new TicketEntity
        {
            Id = NextTicketId(),
            ProjectId = projectId,
            SprintId = 1,
            StatusId = STATUS_ID_TODO,
            Type = TicketType.Task,
            Title = "Setup Build Pipeline",
            Description = GetDescription("Setup Build Pipeline"),
            CreatedBy = USER_ID_ALI,
            CreatedDate = new DateTimeOffset(2026, 3, 10, 8, 15, 0, TimeSpan.Zero)
        });

        modelBuilder.Entity<TicketEntity>().HasData(new TicketEntity
        {
            Id = NextTicketId(),
            ProjectId = projectId,
            SprintId = 1,
            StatusId = STATUS_ID_IN_PROGRESS,
            AssigneeId = USER_ID_DENIZ,
            Type = TicketType.Task,
            Title = "Setup Design System",
            Description = GetDescription("Setup Design System"),
            CreatedBy = USER_ID_ALI,
            CreatedDate = new DateTimeOffset(2026, 3, 10, 8, 15, 0, TimeSpan.Zero)
        });
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

    private int NextTicketId()
    {
        TicketIdCounter += 1;
        int nextId = TicketIdCounter;
        return nextId;
    }
}