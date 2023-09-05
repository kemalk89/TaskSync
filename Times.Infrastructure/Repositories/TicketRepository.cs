using Microsoft.EntityFrameworkCore;
using Times.Domain.Exceptions;
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

    public async Task<TicketModel?> CreateAsync(CreateTicketCommand cmd)
    {
        var project = await _dbContext.Projects.FindAsync(cmd.ProjectId);
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

            return new TicketModel
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

    public async Task<PagedResult<TicketModel>> GetByProjectIdAsync(int projectId, int pageNumber, int pageSize)
    {
        return await GetByFilter(
            pageNumber: pageNumber,
            pageSize: pageSize,
            projectId: projectId);
    }

    public async Task<PagedResult<TicketModel>> GetAllAsync(int pageNumber, int pageSize)
    {
        return await GetByFilter(
            pageNumber: pageNumber,
            pageSize: pageSize);
    }

    public async Task<TicketModel?> GetByIdAsync(int id)
    {
        var tickets = await GetByFilter(
            pageNumber: 1,
            pageSize: 1,
            ticketId: id);

        return tickets.Items.FirstOrDefault();
    }

    public async Task<TicketStatus> UpdateTicketStatusAsync(int ticketId, int statusId)
    {
        var ticket = await _dbContext.Tickets.FindAsync(ticketId);
        if (ticket == null)
        {
            throw new ResourceNotFoundException($"Ticket with id {ticketId} could not be found.");
        }

        var status = _dbContext.TicketStatus.Find(statusId);
        if (status == null)
        {
            throw new ResourceNotFoundException($"TicketStatus with id {statusId} could not be found.");
        }

        ticket.Status = status;

        await _dbContext.SaveChangesAsync();

        return status.ToDomainObject();
    }

    private async Task<PagedResult<TicketModel>> GetByFilter(
        int pageNumber,
        int pageSize,
        int? projectId = null,
        int? ticketId = null
    )
    {
        var skip = (pageNumber - 1) * pageSize;

        var dbSet = _dbContext.Tickets;
        IQueryable<TicketEntity> query = dbSet.Where(t => 1 == 1);
        if (projectId != null)
        {
            query = dbSet.Where(t => t.ProjectId == projectId);
        }

        if (ticketId != null)
        {
            query = dbSet.Where(t => t.Id == ticketId);
        }

        var tickets = query
            .OrderBy(t => t.CreatedDate)
            .Include(t => t.Project)
            .Include(t => t.Status)
            .Skip(skip)
            .Take(pageSize)
            .ToList();

        var assigneeIds = tickets
            .Where(t => t.HasAssignee())
            .Select(t => t.AssigneeId);

        var createdByIds = tickets.Select(t => t.CreatedBy);

        var userIds = assigneeIds.Union(createdByIds);

        User[]? users = null;
        if (userIds.Count() > 0)
        {
            users = await _userRepository.FindUsersAsync(userIds.ToArray());
        }

        var result = new List<TicketModel>();
        foreach (var ticket in tickets)
        {
            User? createdBy = users?.FirstOrDefault(a => a.Id == ticket.CreatedBy);

            if (ticket.HasAssignee())
            {
                User? assignee = users?.FirstOrDefault(a => a.Id == ticket.AssigneeId);
                result.Add(ticket.ToTicket(assignee: assignee, createdBy: createdBy));
            }
            else
            {
                result.Add(ticket.ToTicket(createdBy: createdBy));
            }
        }

        int total = query.Count();
        var paged = new PagedResult<TicketModel>
        {
            Items = result,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Total = total
        };

        return paged;
    }
}

