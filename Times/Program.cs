using Serilog;
using Times.Domain.Ticket;
using Times.Infrastructure;
using Times.Infrastructure.Repositories;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information("Starting service.");

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllersWithViews();
    builder.Services.AddSwaggerGen();

    ////////////////////////////////////////////////////////////////
    // start: dependency injection
    // TODO introduce Startup.cs later...
    builder.Services.AddScoped<ITicketService, TicketService>();
    builder.Services.AddScoped<ITicketRepository, TicketRepository>();
    builder.Services.AddDbContext<DatabaseContext>();
    // end: dependency injection
    ////////////////////////////////////////////////////////////////

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");

    app.MapFallbackToFile("index.html");

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Application terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}
