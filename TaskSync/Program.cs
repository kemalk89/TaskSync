using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using TaskSync.Domain.User;
using TaskSync.Domain.Project;
using TaskSync.Domain.Ticket;
using TaskSync.Infrastructure;
using TaskSync.Infrastructure.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TaskSync.Auth.Auth0;
using TaskSync.Common;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddEnvironmentVariables()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
{
    DotEnv.Load(msg => Log.Logger.Information(msg));   
}

try
{
    Log.Information("Starting service.");

    var builder = WebApplication.CreateBuilder(args);

    ////////////////////////////////////////////////////////////////
    // start: dependency injection
    // Add services to the container.

    builder.Services.AddControllersWithViews();
    builder.Services.AddSwaggerGen();
    builder.Services.AddHealthChecks();

    builder.Services.AddScoped<IProjectService, ProjectService>();
    builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

    builder.Services.AddScoped<ITicketService, TicketService>();
    builder.Services.AddScoped<ITicketRepository, TicketRepository>();

    builder.Services.AddScoped<IUserService, UserService>();

    builder.Services.AddAuth0();

    builder.Services.AddDbContext<DatabaseContext>(
        o => o.UseNpgsql(configuration.GetConnectionString("db")));
    //.LogTo(s => System.Diagnostics.Debug.WriteLine(s)));

    builder.Services.AddHttpContextAccessor();
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = configuration["Auth:Authority"];
            options.Audience = configuration["Auth:Audience"];
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = ClaimTypes.NameIdentifier
            };
            Log.Information(configuration["Auth:Authority"]);
        });
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
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");

    app.UseExceptionHandler(
        exceptionHandlerApp => exceptionHandlerApp.Run(
            async context => await Results.Problem().ExecuteAsync(context)
        )
    );

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
