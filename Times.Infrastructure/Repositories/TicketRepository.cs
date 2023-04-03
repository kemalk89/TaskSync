using Microsoft.EntityFrameworkCore;
using Times.Domain.Project;
using Times.Domain.Shared;
using Times.Domain.Ticket;
using Times.Domain.Ticket.Command;
using Times.Domain.User;
using Times.Infrastructure.Entities;

namespace Times.Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    private IUserRepository _userRepository;
    private readonly DatabaseContext _dbContext;
    private readonly IProjectRepository _projectRepository;

    public TicketRepository(DatabaseContext dbContext, IProjectRepository projectRepository, IUserRepository userRepository)
    {
        _dbContext = dbContext;
        _projectRepository = projectRepository;
        _userRepository = userRepository;
    }

    public async Task<Ticket?> CreateAsync(CreateTicketCommand cmd)
    {
        var project = _dbContext.Projects.Find(cmd.ProjectId);
        if (project != null)
        {
            var ticket = new TicketEntity
            {
                Title = cmd.Title,
                Description = cmd.Description,
                Project = project,
                AssigneeId = cmd.Assignee?.Id
            };

            await _dbContext.Tickets.AddAsync(ticket);
            await _dbContext.SaveChangesAsync();

            return new Ticket
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Assignee = cmd.Assignee,
                ProjectId = ticket.Project.Id,
                Project = ticket.Project.ToDomainObject(),
            };
        }

        return null;
    }

    public async Task<PagedResult<Ticket>> GetAllAsync(int pageNumber, int pageSize)
    {
        var skip = (pageNumber - 1) * pageSize;

        var tickets = _dbContext.Tickets
            .OrderBy(t => t.Title)
            .Include(t => t.Project)
            .Include(t => t.Status)
            .Skip(skip)
            .Take(pageSize)
            .ToList();

        var assigneeIds = tickets
            .Where(t => t.HasAssignee())
            .Select(t => t.AssigneeId);

        User[]? assignees = null;
        if (assigneeIds.Count() > 0)
        {
            assignees = await _userRepository.FindUsersAsync(assigneeIds.ToArray());
        }

        var result = new List<Ticket>();
        foreach (var ticket in tickets)
        {
            if (ticket.HasAssignee())
            {
                User? assignee = assignees?.FirstOrDefault(a => a.Id == ticket.AssigneeId);
                result.Add(ticket.ToTicket(assignee: assignee));
            }
            else
            {
                result.Add(ticket.ToTicket());
            }
        }

        int total = _dbContext.Tickets.Count();
        var paged = new PagedResult<Ticket>
        {
            Items = result,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Total = total
        };

        return paged;
    }

    public async Task<Ticket?> GetByIdAsync(int id)
    {
        TicketEntity? entity = await _dbContext.Tickets
            .Where(t => t.Id == id)
            .Include(t => t.Project)
            .Include(t => t.Status)
            .FirstAsync();

        if (entity == null)
        {
            return null;
        }

        List<string> userIds = new List<string> { entity.CreatedBy };

        if (entity.HasAssignee())
        {
            userIds.Add(entity.AssigneeId);
        }

        var users = await _userRepository.FindUsersAsync(userIds.ToArray());

        var createdBy = users.FirstOrDefault(u => u.Id == entity.CreatedBy);
        var assignee = users.FirstOrDefault(u => u.Id == entity.AssigneeId);

        return entity.ToTicket(createdBy, assignee);
    }
}
