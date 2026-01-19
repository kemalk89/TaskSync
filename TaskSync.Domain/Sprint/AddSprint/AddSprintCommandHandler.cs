using TaskSync.Domain.Shared;

namespace TaskSync.Domain.Sprint.AddSprint;

public class AddSprintCommandHandler : ICommandHandler
{
    private readonly ISprintRepository _sprintRepository;

    public AddSprintCommandHandler(ISprintRepository sprintRepository)
    {
        _sprintRepository = sprintRepository;
    }

    public async Task<Result<SprintModel>> HandleAsync(AddSprintCommand cmd, CancellationToken cancellationToken)
    {
        var result = await _sprintRepository.CreateAsync(cmd, cancellationToken);
        return result;
    }
    
}