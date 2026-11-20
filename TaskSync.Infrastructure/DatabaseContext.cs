using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using TaskSync.Infrastructure.Entities;
using TaskSync.Infrastructure.Services;

namespace TaskSync.Infrastructure;

public class DatabaseContext : IdentityDbContext<UserEntity>
{
    public DbSet<TicketEntity> Tickets { get; set; }
    public DbSet<TicketCommentEntity> TicketComments { get; set; }
    public DbSet<TicketStatusEntity> TicketStatus { get; set; }
    public DbSet<TicketLabelEntity> TicketLabels { get; set; }

    public DbSet<ProjectEntity> Projects { get; set; }
    public DbSet<SprintEntity> Sprints { get; set; }
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHostEnvironment _env;
    private readonly IConfiguration _config;
    private readonly ILogger<DatabaseContext> _logger;


    public DatabaseContext(
        DbContextOptions<DatabaseContext> options, 
        IHttpContextAccessor httpContextAccessor, 
        IHostEnvironment env, 
        IConfiguration config, 
        ILogger<DatabaseContext> logger)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        _env = env;
        _config = config;
        _logger = logger;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
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
            if (e is IAuditedEntity audited)
            {
                var currentUser = await TryGetCurrentUserAsync(audited, cancellationToken);
                audited.CreatedBy = currentUser?.Id ?? string.Empty;
                audited.CreatedDate = DateTimeOffset.UtcNow;
            }
        }

        foreach (var e in modified)
        {
            if (e is AuditedEntity audited)
            {
                // TODO Add ModifiedBy here
                audited.ModifiedDate = DateTimeOffset.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task<UserEntity?> TryGetCurrentUserAsync(IAuditedEntity audited, CancellationToken cancellationToken)
    {
        UserEntity? currentUser = null;
        if (_httpContextAccessor.HttpContext != null)
        {
            var authSchemeName =
                AuthSchemeNameService.GetAuthSchemeNameByRequest(_httpContextAccessor.HttpContext,
                    _config);
            
            var userId = _httpContextAccessor.HttpContext?.User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogInformation("Cannot read current user because no user id given");
            }
            else
            {
                if (authSchemeName == AuthSchemeNameService.AuthSchemeNameAuth0)
                {
                    currentUser = await Users.FirstOrDefaultAsync(u => u.ExternalUserId == userId, cancellationToken);
                }
                else if (authSchemeName == IdentityConstants.BearerScheme)
                {
                    currentUser = await Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
                }
                
                if (currentUser == null)
                {
                    _logger.LogInformation("No user found for id {UserId}", userId);
                }
            }
        }

        if (currentUser == null)
        {
            // Throw an exception unless we're inserting a new User record - because in this case the user is not yet available in the users table!
            if (audited.GetType().Name != nameof(UserEntity))
            {
                throw new Exception("No user found");
            }
        }

        return currentUser;
    }
}
