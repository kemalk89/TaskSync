using Microsoft.EntityFrameworkCore;

using TaskSync.Domain.Exceptions;
using TaskSync.Domain.Project;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket;
using TaskSync.Domain.Ticket.Command;
using TaskSync.Domain.User;
using TaskSync.Infrastructure.Entities;

namespace TaskSync.Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    private IUserRepository _userRepository;
    private readonly DatabaseContext _dbContext;
    private readonly IProjectRepository _projectRepository;

    public TicketRepository(DatabaseContext dbContext, IProjectRepository projectRepository,
        IUserRepository userRepository)
    {
        _dbContext = dbContext;
        _projectRepository = projectRepository;
        _userRepository = userRepository;
    }

    public async Task<int> DeleteTicketAsync(int id)
    {
        // immediately execute deletion
        var numOfDeletedRecords = await _dbContext.Tickets.Where(t => t.Id == id).ExecuteDeleteAsync();
        return numOfDeletedRecords;
    }
    
    public async Task<int?> CreateAsync(CreateTicketCommand cmd)
    {
        var ticketType = TicketType.Task;
        var hasParsed = Enum.TryParse<TicketType>(cmd.Type, true, out var parsedTicketType);
        if (hasParsed)
        {
            ticketType = parsedTicketType;
        }

        var project = await _dbContext.Projects.FindAsync(cmd.ProjectId);
        if (project == null)
        {
            return null;
        }

        var ticket = new TicketEntity
        {
            Title = cmd.Title,
            Description = cmd.Description,
            Project = project,
            AssigneeId = cmd.Assignee,
            Type = ticketType
        };

        await _dbContext.Tickets.AddAsync(ticket);
        await _dbContext.SaveChangesAsync();

        return ticket.Id;
    }

    public async Task<PagedResult<TicketModel>> GetByProjectIdAsync(int projectId, int pageNumber, int pageSize)
    {
        return await GetByFilter(
            pageNumber: pageNumber,
            pageSize: pageSize,
            projectId: projectId);
    }

    public async Task<PagedResult<TicketModel>> GetAllAsync(int pageNumber, int pageSize, TicketSearchFilter filter)
    {
        return await GetByFilter(
            pageNumber: pageNumber,
            pageSize: pageSize,
            filter: filter);
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

    public async Task<TicketCommentModel> AddTicketCommentAsync(int ticketId, CreateTicketCommentCommand cmd)
    {
        var ticket = await _dbContext.Tickets.FindAsync(ticketId);
        if (ticket == null)
        {
            throw new ResourceNotFoundException($"Ticket with id {ticketId} could not be found.");
        }

        var comment = new TicketCommentEntity { Comment = cmd.Comment, TicketId = ticketId };

        await _dbContext.TicketComments.AddAsync(comment);
        await _dbContext.SaveChangesAsync();

        var author = await _userRepository.FindUserByIdAsync(comment.CreatedBy);

        return comment.ToModel(author);
    }

    public async Task<PagedResult<TicketCommentModel>> GetTicketCommentsAsync(int ticketId, int pageNumber,
        int pageSize)
    {
        var skip = (pageNumber - 1) * pageSize;
        var query = _dbContext.TicketComments
            .Where(comment => comment.TicketId == ticketId);

        var comments = query
            .Skip(skip)
            .Take(pageSize)
            .ToList();

        var createdByIds = comments.Select(t => t.CreatedBy);
        var users = await _userRepository.FindUsersAsync(createdByIds.ToArray());

        var result = new List<TicketCommentModel>();

        foreach (var comment in comments)
        {
            var author = users?.FirstOrDefault(a => a.Id == comment.CreatedBy);
            result.Add(comment.ToModel(author));
        }

        var total = query.Count();
        var paged = new PagedResult<TicketCommentModel>
        {
            Items = result, PageNumber = pageNumber, PageSize = pageSize, Total = total
        };

        return paged;
    }

    private async Task<PagedResult<TicketModel>> GetByFilter(
        int pageNumber,
        int pageSize,
        int? projectId = null,
        int? ticketId = null,
        TicketSearchFilter? filter = null
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

        if (filter != null && !string.IsNullOrWhiteSpace(filter.SearchText))
        {
            query = dbSet.Where(t => t.Title.Contains(filter.SearchText));
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
            .Select(t => t.AssigneeId.Value);

        var createdByIds = tickets.Select(t => t.CreatedBy);

        var userIds = assigneeIds.Union<int>(createdByIds);

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
            Items = result, PageNumber = pageNumber, PageSize = pageSize, Total = total
        };

        return paged;
    }
}