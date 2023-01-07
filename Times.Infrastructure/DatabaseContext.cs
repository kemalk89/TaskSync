using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Times.Infrastructure.Entities;

namespace Times.Infrastructure;

public class DatabaseContext : DbContext
{
    public DbSet<TicketEntity> Tickets { get; set; }

    private readonly IConfiguration _config;

    public DatabaseContext(IConfiguration config)
    {
        _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connString = _config["DB:ConnectionString"];

        options.UseNpgsql(connString);
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
