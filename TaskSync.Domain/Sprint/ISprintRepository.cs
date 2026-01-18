using TaskSync.Domain.Sprint.AddSprint;

namespace TaskSync.Domain.Sprint;

public interface ISprintRepository
{
    Task<SprintModel> CreateAsync(AddSprintCommand command, CancellationToken cancellationToken);
}