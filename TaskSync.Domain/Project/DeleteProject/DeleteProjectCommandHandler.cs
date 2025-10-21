using TaskSync.Domain.Shared;

namespace TaskSync.Domain.Project.DeleteProject;

public class DeleteProjectCommandHandler : ICommandHandler
{
    private readonly IProjectRepository _projectRepository;

    public DeleteProjectCommandHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task HandleAsync(int id)
    {
        await _projectRepository.DeleteByIdAsync(id);
    }
}