namespace TaskSync.Domain.Shared;

/// <summary>
/// Marker interface used to identify a class as a command handler within an assembly.
/// Implementing this interface enables automatic discovery and registration of the
/// handler type in the dependency injection container.
/// </summary>
public interface ICommandHandler {
}