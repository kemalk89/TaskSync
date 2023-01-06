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
}
