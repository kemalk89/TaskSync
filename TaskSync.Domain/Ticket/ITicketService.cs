using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket.Command;

namespace TaskSync.Domain.Ticket;

public interface ITicketService
{
    Task<TicketModel?> CreateTicketAsync(CreateTicketCommand cmd);
    Task<TicketModel?> GetTicketByIdAsync(int id);
    Task<PagedResult<TicketModel>> GetTicketsAsync(int pageNumber, int pageSize);
    Task<PagedResult<TicketModel>> GetTicketsByProjectIdAsync(int projectId, int pageNumber, int pageSize);
    Task<TicketStatus> UpdateTicketStatusAsync(int ticketId, int statusId);
    Task<TicketCommentModel> AddCommentAsync(int id, CreateTicketCommentCommand cmd);
    Task<PagedResult<TicketCommentModel>> GetTicketCommentsAsync(int id, int pageNumber, int pageSize);
}
