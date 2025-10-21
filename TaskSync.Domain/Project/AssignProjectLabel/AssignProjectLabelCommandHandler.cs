using FluentValidation;

using TaskSync.Domain.Shared;

namespace TaskSync.Domain.Project.AssignProjectLabel;

public class AssignProjectLabelCommandHandler : ICommandHandler
{
    private readonly IValidator<AssignProjectLabelCommand> _createProjectLabelCommandValidator;
    private readonly IProjectRepository _projectRepository;

    public AssignProjectLabelCommandHandler(
        IValidator<AssignProjectLabelCommand> createProjectLabelCommandValidator, 
        IProjectRepository projectRepository
    )
    {
        _createProjectLabelCommandValidator = createProjectLabelCommandValidator;
        _projectRepository = projectRepository;
    }

    public async Task<Result<int>> HandleAsync(AssignProjectLabelCommand command)
    {
        var result = await _createProjectLabelCommandValidator.ValidateAsync(command);
        if (!result.IsValid)
        {
            return Result<int>.Fail(ResultCodes.ResultCodeValidationFailed, result);
        }

        var project = await _projectRepository.GetByIdAsync(command.ProjectId);
        if (project == null)
        {
            return Result<int>.Fail(ResultCodes.ResultCodeValidationFailed, "Project not found. ID " + command.ProjectId);
        }

        var labelId = await _projectRepository.CreateLabelAsync(command.ProjectId, command.Text);
        return Result<int>.Ok(labelId);
    }
}