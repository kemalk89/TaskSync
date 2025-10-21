using TaskSync.Domain.Shared;

namespace TaskSync.Extensions;

/// <summary>
/// Provides extension method for automatically registering command handler classes 
/// that implement the <see cref="ICommandHandler"/> marker interface.
/// </summary>
public static class CommandHandlerDependenciesExtensions
{
    /// <summary>
    /// Scans the assembly containing the specified type <typeparamref name="T"/> for 
    /// all non-abstract, public classes that implement the <see cref="ICommandHandler"/> 
    /// marker interface, and registers each of them in the dependency injection container
    /// with a <see cref="ServiceLifetime.Scoped"/> lifetime.
    /// </summary>
    /// <typeparam name="TCommandHandler">
    /// A type that resides in the assembly to be scanned for command handlers.  
    /// This type is used only to locate the target assembly.
    /// </typeparam>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to which the discovered command handlers 
    /// will be added.
    /// </param>
    /// <returns>
    /// <see cref="IServiceCollection"/>
    /// </returns>
    public static IServiceCollection AddCommandHandlersFromAssemblyContaining<TCommandHandler>(
        this IServiceCollection services) 
    {
        var commandHandlers = typeof(TCommandHandler).Assembly.GetTypes()
            .Where(type => type.IsClass
                           && !type.IsAbstract
                           && type.IsPublic
                           && typeof(ICommandHandler).IsAssignableFrom(type)
            ).ToList();

        foreach (var commandHandler in commandHandlers)
        {
            services.AddScoped(commandHandler);
        }
        
        return services;
    }
}