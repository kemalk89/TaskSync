using Microsoft.EntityFrameworkCore;

using TaskSync.Domain.Exceptions;
using TaskSync.Domain.Project.ReorderBacklogTickets;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket;
using TaskSync.Domain.Ticket.Command;
using TaskSync.Domain.Ticket.CreateTicket;
using TaskSync.Domain.Ticket.QueryTicket;
using TaskSync.Domain.Ticket.UpdateTicket;
using TaskSync.Domain.User;
using TaskSync.Infrastructure.Entities;

namespace TaskSync.Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    private IUserRepository _userRepository;
    private readonly DatabaseContext _dbContext;

    public TicketRepository(DatabaseContext dbContext,
        IUserRepository userRepository)
    {
        _dbContext = dbContext;
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
            Type = ticketType,
            StatusId = cmd.StatusId
        };

        await _dbContext.Tickets.AddAsync(ticket);
        await _dbContext.SaveChangesAsync();

        return ticket.Id;
    }

    public async Task<PagedResult<TicketModel>> GetByProjectIdAsync(int projectId, int pageNumber, int pageSize)
    {
        return await GetByFilterAsync(
            pageNumber: pageNumber,
            pageSize: pageSize,
            projectId: projectId);
    }

    public async Task<PagedResult<TicketModel>> GetAllAsync(
        int pageNumber, int pageSize, TicketSearchFilter filter, CancellationToken cancellationToken)
    {
        return await GetByFilterAsync(
            pageNumber: pageNumber,
            pageSize: pageSize,
            filter: filter);
    }

    public async Task<List<TicketModel>> GetAllAsync(TicketSearchFilter filter, CancellationToken cancellationToken)
    {
        return await GetByFilterAndMapAsync(filter, cancellationToken);
    }

    public async Task<TicketModel?> GetByIdAsync(int id)
    {
        var tickets = await GetByFilterAsync(
            pageNumber: 1,
            pageSize: 1,
            ticketId: id);

        return tickets.Items.FirstOrDefault();
    }

    public async Task<TicketCommentModel> AddTicketCommentAsync(int ticketId, AddTicketCommentCommand cmd)
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
            .Where(comment => comment.TicketId == ticketId)
            .OrderByDescending((comment => comment.CreatedDate));

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

    private async Task<List<TicketModel>> GetByFilterAndMapAsync(
        TicketSearchFilter filter,         
        CancellationToken cancellationToken)
    {
        var tickets = await GetByFilterAsync(filter, cancellationToken);
        return tickets.Select(e => e.ToTicket()).ToList();
    }
    
    private async Task<List<TicketEntity>> GetByFilterAsync(
        TicketSearchFilter filter, 
        CancellationToken cancellationToken)
    {
        IQueryable<TicketEntity> query = UpdateQueryByFilter(_dbContext.Tickets.AsQueryable(), filter);

        if (filter.OrderBy is not null && filter.OrderBy.Equals(TicketModel.OrderByPosition))
        {
            query = query.OrderBy(t => t.Position);
        }
        else
        {
            query = query.OrderBy(t => t.CreatedDate);
        }

        
        var tickets = await query
            .ToListAsync(cancellationToken);

        return tickets;
    }

    
    private async Task<PagedResult<TicketModel>> GetByFilterAsync(
        int pageNumber,
        int pageSize,
        int? projectId = null,
        int? ticketId = null,
        TicketSearchFilter? filter = null
    )
    {
        var skip = (pageNumber - 1) * pageSize;

        IQueryable<TicketEntity> query = _dbContext.Tickets.AsQueryable();
        if (projectId is not null)
        {
            query = query.Where(t => t.ProjectId == projectId);
        }

        if (ticketId is not null)
        {
            query = query.Where(t => t.Id == ticketId);
        }

        if (filter is not null)
        {
            query = UpdateQueryByFilter(query, filter);
        }

        var tickets = await query
            .OrderBy(t => t.CreatedDate)
            .Include(t => t.Project)
            .Include(t => t.Status)
            .Include(t => t.Labels)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();

        List<int> assigneeIds = [];
        foreach (var t in tickets)
        {
            if (t.AssigneeId != null)
            {
                assigneeIds.Add(t.AssigneeId.Value);
            }
        }

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

            if (ticket.AssigneeId is not null)
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

    
    private IQueryable<TicketEntity> UpdateQueryByFilter(IQueryable<TicketEntity> query, TicketSearchFilter filter)
    {
        IQueryable<TicketEntity> updatedQuery = query;
        if (!string.IsNullOrWhiteSpace(filter.SearchText))
        {
            updatedQuery = updatedQuery.Where(t => EF.Functions.Like(t.Title.ToLower(), $"%{filter.SearchText.ToLower()}%"));
        }

        if (filter.StatusIds.Count > 0)
        {
            updatedQuery = updatedQuery.Where(t => t.StatusId.HasValue && filter.StatusIds.Contains(t.StatusId.Value));
        }
        
        if (filter.TicketIds.Count > 0)
        {
            updatedQuery = updatedQuery.Where(t => filter.TicketIds.Contains(t.Id));
        }
        
        if (filter.ProjectIds.Count > 0)
        {
            updatedQuery = updatedQuery.Where(t => filter.ProjectIds.Contains(t.ProjectId));
        }
        
        if (filter.AssigneeIds.Count > 0)
        {
            updatedQuery = updatedQuery.Where(t => t.AssigneeId.HasValue && filter.AssigneeIds.Contains(t.AssigneeId.Value));
        }

        if (filter.OnlyBacklogTickets)
        {
            updatedQuery = updatedQuery.Where(t => t.SprintId == null);
        } else if (filter.BoardId > 0)
        {
            updatedQuery = updatedQuery.Where(t => t.SprintId == filter.BoardId);
        }
        
        return updatedQuery;
    }

    public async Task<TicketCommentModel?> GetTicketCommentByIdAsync(int commentId)
    {
        var entity = await _dbContext.TicketComments.FindAsync(commentId);
        return entity?.ToModel();
    }

    public async Task<bool> DeleteTicketCommentAsync(int commentId)
    {
        var comment = await _dbContext.TicketComments.FindAsync(commentId);
        if (comment is not null)
        {
            comment.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<Result<bool>> UpdateTicketAsync(int ticketId, UpdateTicketCommand updateTicketCommand)
    {
        var ticket = await _dbContext.Tickets.FindAsync(ticketId);
        if (ticket is null)
        { 
            return Result<bool>.Fail($"Ticket with id {ticketId} could not be found.");
        }

        if (updateTicketCommand.StatusId != null)
        {
            var status = _dbContext.TicketStatus.Find(updateTicketCommand.StatusId);
            if (status is null)
            {
                return Result<bool>.Fail($"TicketStatus with id {updateTicketCommand.StatusId} could not be found.");
            }
            
            ticket.Status = status;
        }

        if (!string.IsNullOrWhiteSpace(updateTicketCommand.Title))
        {
            ticket.Title = updateTicketCommand.Title;
        }
        
        if (updateTicketCommand.Description is not null)
        {
            ticket.Description = updateTicketCommand.Description;
        }

        await _dbContext.SaveChangesAsync();
        return Result<bool>.Ok(true);
    }

    public async Task<Result<int>> AssignTicketLabelAsync(int projectId, int ticketId, int labelId)
    {
        var ticket = await _dbContext.Tickets.FindAsync(ticketId);
        if (ticket is null)
        { 
            return Result<int>.Fail($"Ticket with id {ticketId} could not be found.");
        }

        var label = await _dbContext.TicketLabels.FindAsync(labelId);
        if (label is null)
        {
            return Result<int>.Fail($"Ticket Label with id {labelId} could not be found.");
        }

        if (!ticket.Labels.Any(e => e.Id == label.Id))
        {
            ticket.Labels.Add(label);
            await _dbContext.SaveChangesAsync();
        }
        
        return Result<int>.Ok(labelId);
    }

    public async Task<List<TicketStatusModel>> GetTicketStatusListAsync(CancellationToken cancellationToken)
    {
        var entities = await _dbContext.TicketStatus
            .ToListAsync(cancellationToken);
        
        return entities.Select(e => e.ToDomainObject()).ToList();
    }

    public async Task<List<TicketModel>> GetBacklogTicketsAsync(int projectId, CancellationToken cancellationToken)
    {
        var filter = new TicketSearchFilter
        {
            ProjectIds = [projectId], OnlyBacklogTickets = true, OrderBy = TicketModel.OrderByPosition
        };
        return await GetByFilterAndMapAsync(filter,  cancellationToken);
    }

    public async Task<Result<int>> ReorderBoardTickets(
        int projectId, 
        int? boardId,
        List<ReorderTicketCommand> ticketOrders, 
        CancellationToken cancellationToken
    )
    {
        var ticketIds = ticketOrders.Select(i => i.TicketId).ToList();
        var filter = new TicketSearchFilter { ProjectIds = [projectId], TicketIds = ticketIds };
        var tickets = await GetByFilterAsync(filter, cancellationToken);
        foreach (var ticketOrder in ticketOrders)
        {
            var foundTicket = tickets.Find(e => e.Id == ticketOrder.TicketId);
            if (foundTicket is not null)
            {
                foundTicket.Position = ticketOrder.Position;
                foundTicket.SprintId = boardId;
            }
        }

        var numberOfAffectedTickets = await _dbContext.SaveChangesAsync(cancellationToken);
        return Result<int>.Ok(numberOfAffectedTickets);
    }
}