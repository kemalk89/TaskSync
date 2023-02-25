using Microsoft.EntityFrameworkCore;
using Times.Infrastructure.Entities;

namespace Times.Infrastructure;

public class DatabaseContext : DbContext
{
    public DbSet<TicketEntity> Tickets { get; set; }
    public DbSet<ProjectEntity> Projects { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TicketEntity>()
            .HasOne(t => t.Project)
            .WithMany()
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
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
