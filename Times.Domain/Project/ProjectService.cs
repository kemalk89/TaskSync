using Times.Domain.Shared;

namespace Times.Domain.Project;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Project> CreateProjectAsync(string title, string? description)
    {
        var project = await _projectRepository.CreateAsync(title, description);
        return project;
    }

    public async Task<Project?> GetProjectByIdAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        return project;
    }

    public async Task<PagedResult<Project>> GetProjectsAsync(int pageNumber, int pageSize)
    {
        var projects = await _projectRepository.GetAllAsync(pageNumber, pageSize);
        return projects;
    }

    public async Task DeleteProjectAsync(int id)
    {
        await _projectRepository.DeleteByIdAsync(id);
    }
}
