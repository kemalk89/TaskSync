using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket.Command;
using TaskSync.Domain.Ticket.CreateTicket;
using TaskSync.Domain.Ticket.QueryTicket;
using TaskSync.Domain.Ticket.UpdateTicket;

namespace TaskSync.Domain.Ticket;

public interface ITicketRepository
{
    Task<int?> CreateAsync(CreateTicketCommand cmd);
    Task<TicketModel?> GetByIdAsync(int id);
    Task<PagedResult<TicketModel>> GetByProjectIdAsync(int projectId, int pageNumber, int pageSize);
    Task<PagedResult<TicketModel>> GetAllAsync(
        int pageNumber, int pageSize, TicketSearchFilter filter, CancellationToken cancellationToken);
    Task<List<TicketModel>> GetAllAsync(TicketSearchFilter filter, CancellationToken cancellationToken);
    Task<Result<bool>> UpdateTicketAsync(int ticketId, UpdateTicketCommand updateTicketCommand);
    Task<TicketCommentModel> AddTicketCommentAsync(int ticketId, AddTicketCommentCommand cmd);
    Task<TicketCommentModel?> GetTicketCommentByIdAsync(int commentId);
    Task<PagedResult<TicketCommentModel>> GetTicketCommentsAsync(int id, int pageNumber, int pageSize);
    Task<int> DeleteTicketAsync(int id);
    Task<bool> DeleteTicketCommentAsync(int commentId);
    Task<Result<int>> AssignTicketLabelAsync(int projectId, int ticketId, int labelId);
    Task<List<TicketStatusModel>> GetTicketStatusListAsync(CancellationToken cancellationToken);
    Task<List<TicketModel>> GetBacklogTicketsAsync(int projectId, CancellationToken cancellationToken);
    Task<Result<int>> ReorderBacklogTickets(
        int projectId, Dictionary<int, int> ticketOrders, CancellationToken cancellationToken);
}
