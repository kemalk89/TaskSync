using FluentValidation;

using TaskSync.Domain.Shared;

namespace TaskSync.Domain.Sprint.AddSprint;

public class AddSprintCommandHandler : ICommandHandler
{
    private readonly ISprintRepository _sprintRepository;
    private readonly IValidator<AddSprintCommand> _validator;

    public AddSprintCommandHandler(
        IValidator<AddSprintCommand> validator, ISprintRepository sprintRepository)
    {
        _validator = validator;
        _sprintRepository = sprintRepository;
    }

    public async Task<Result<SprintModel>> HandleAsync(AddSprintCommand cmd, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(cmd);
        if (!validationResult.IsValid)
        {
            return Result<SprintModel>.Fail(ResultCodes.ResultCodeValidationFailed, validationResult);
        }

        var result = await _sprintRepository.CreateAsync(cmd, cancellationToken);
        return result;
    }

}