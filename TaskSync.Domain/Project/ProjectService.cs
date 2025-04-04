using TaskSync.Domain.Project.Commands;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket;
using TaskSync.Domain.User;

namespace TaskSync.Domain.Project;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly ITicketService _ticketService;
    private readonly IUserService _userService;

    public ProjectService(IProjectRepository projectRepository, ITicketService ticketService, IUserService userService)
    {
        _projectRepository = projectRepository;
        _ticketService = ticketService;
        _userService = userService;
    }

    public async Task<Project> CreateProjectAsync(CreateProjectCommand command)
    {
        var project = await _projectRepository.CreateAsync(command);
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

    public async Task<PagedResult<TicketModel>> GetProjectTicketsAsync(int projectId, int pageNumber = 1, int pageSize = 0)
    {
        var project = await GetProjectByIdAsync(projectId);
        if (project == null)
        {
            return new PagedResult<TicketModel>
            {
                Total = 0,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        return await _ticketService.GetTicketsByProjectIdAsync(projectId, pageNumber, pageSize);
    }
    
    public async Task AssignTeamMembersAsync(int projectId, AssignTeamMembersCommand command)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
        {
            // TODO Implement return
            throw new NotImplementedException();
        }

        command.TeamMembers.ToList().ForEach(m => project.ProjectMembers.Add(new ProjectMember
        {
            UserId = m.UserId,
            Role = m.Role,
        }));

        await _projectRepository.SaveAsync(project);
    }
}

