using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

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

        var dataSeeding = new DataSeeding(_env);
        dataSeeding.Seed(modelBuilder);
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
                UserEntity? currentUser = null;
                string userIdentifier = string.Empty;

                // Try to get current user by claim "id" (for local users)
                var claimId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == "id");
                var userId = claimId?.Value;
                if (userId != null && int.TryParse(userId, out int parsed))
                {
                    userIdentifier = userId;
                    currentUser = await Users.FirstOrDefaultAsync(u => u.Id == parsed, cancellationToken);
                }
                else
                {
                    // Try to get current user by claim identity name (for external users)
                    var identityName = _httpContextAccessor.HttpContext?.User.Identity?.Name;
                    if (identityName == null)
                    {
                        throw new Exception("No authenticated user");
                    }

                    userIdentifier = identityName;
                    currentUser = await Users.FirstOrDefaultAsync(u => u.ExternalUserId == identityName, cancellationToken);
                }

                if (currentUser == null)
                {
                    // Throw an exception unless we're inserting a new User record - because in this case the user is not yet available in the users table!
                    if (audited.GetType().Name != nameof(UserEntity))
                    {
                        throw new Exception($"No user found for identifier {userIdentifier}");
                    }
                }
                else
                {
                    var currentUserId = 0;
                    currentUserId = currentUser.Id;
                    audited.CreatedBy = currentUserId;
                }

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
