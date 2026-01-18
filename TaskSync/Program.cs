using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using TaskSync.Domain.User;
using TaskSync.Domain.Project;
using TaskSync.Domain.Ticket;
using TaskSync.Infrastructure;
using TaskSync.Infrastructure.Repositories;
using TaskSync.Infrastructure.Services;

using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

using FluentValidation;

using Microsoft.OpenApi.Models;

using TaskSync.Auth.Auth0;
using TaskSync.Common;
using TaskSync.Domain.Project.CreateProject;
using TaskSync.Domain.Project.QueryProject;
using TaskSync.Domain.Sprint;
using TaskSync.Extensions;

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
{
    DotEnv.Load(msg => Console.WriteLine(msg));   
}

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
    .AddEnvironmentVariables()
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information("Starting service.");

    var builder = WebApplication.CreateBuilder(args);

    ////////////////////////////////////////////////////////////////
    // start: dependency injection
    // Add services to the container.

    builder.Services.AddControllersWithViews();
    builder.Services.AddSwaggerGen(opt =>
        {
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    builder.Services.AddHealthChecks();

    builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

    builder.Services.AddScoped<ITicketRepository, TicketRepository>();
    builder.Services.AddScoped<ISprintRepository, SprintRepository>();
    
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();

    builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

    builder.Services.AddValidatorsFromAssemblyContaining<CreateProjectCommandValidator>();
    builder.Services.AddCommandHandlersFromAssemblyContaining<QueryProjectCommandHandler>();
    
    builder.Services.AddAuth0();

    builder.Services.AddDbContext<DatabaseContext>(
        o => o.UseNpgsql(configuration.GetConnectionString("db"))
            .LogTo(Console.WriteLine,  LogLevel.Information)
    );

    builder.Services.AddHttpContextAccessor();
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = configuration["Auth:Authority"];
            options.Audience = configuration["Auth:Audience"];
            options.MetadataAddress = configuration["Auth:MetadataAddress"] ?? string.Empty;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = ClaimTypes.NameIdentifier,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true
            };

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                options.RequireHttpsMetadata = false; // Allow HTTP in development
            }
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
        app.ApplyMigrations();
    }

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapHealthChecks("/health");
    
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

// This line of code is needed to enable integration tests
public partial class Program { }