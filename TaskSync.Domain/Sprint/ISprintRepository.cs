using TaskSync.Domain.Shared;
using TaskSync.Domain.Sprint.AddSprint;

namespace TaskSync.Domain.Sprint;

public interface ISprintRepository
{
    Task<Result<SprintModel>> CreateAsync(AddSprintCommand command, CancellationToken cancellationToken);
    Task<Result<bool>> AssignTicketAsync(int sprintId, int ticketId, CancellationToken cancellationToken);
    Task<Result<SprintModel>> GetDraftSprintAsync(int projectId, CancellationToken cancellationToken);
}