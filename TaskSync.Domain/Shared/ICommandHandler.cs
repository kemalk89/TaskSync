namespace TaskSync.Domain.Shared;

public interface ICommandHandler<TCommand, TResult>
{
    Task<Result<TResult>> HandleCommandAsync(TCommand command);
}