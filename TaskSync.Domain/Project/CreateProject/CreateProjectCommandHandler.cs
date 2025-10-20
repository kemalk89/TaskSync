using FluentValidation;

using TaskSync.Domain.Shared;

namespace TaskSync.Domain.Project.CreateProject;

public class CreateProjectCommandHandler : ICommandHandler<CreateProjectCommand, ProjectModel>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IValidator<CreateProjectCommand> _createProjectCommandValidator;

    public CreateProjectCommandHandler(
        IProjectRepository projectRepository, 
        IValidator<CreateProjectCommand> createProjectCommandValidator)
    {
        _projectRepository = projectRepository;
        _createProjectCommandValidator = createProjectCommandValidator;
    }

    public async Task<Result<ProjectModel>> HandleCommandAsync(CreateProjectCommand command)
    {
        var result = await _createProjectCommandValidator.ValidateAsync(command);
        if (!result.IsValid)
        {
            return Result<ProjectModel>.Fail(ResultCodes.ResultCodeValidationFailed, result);
        }
        
        var project = await _projectRepository.CreateAsync(command);
        return Result<ProjectModel>.Ok(project);
    }
}