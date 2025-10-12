using Microsoft.EntityFrameworkCore;

using TaskSync.Infrastructure;

namespace TaskSync.Extensions;

public static class MigrationExtensions
{
    /// <summary>
    /// In development environment (for example local development, or in integration tests) this extension method will
    /// run all migrations.
    /// </summary>
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        dbContext.Database.Migrate();
    }
}