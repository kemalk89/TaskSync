using TaskSync.Domain.Exceptions;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket.Command;

namespace TaskSync.Domain.Ticket.AddTicketComment;

public class AddTicketCommentCommandHandler : ICommandHandler
{
    private readonly ITicketRepository _ticketRepository;

    public AddTicketCommentCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<TicketCommentModel> HandleAsync(int id, AddTicketCommentCommand cmd)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket == null)
        {
            throw new ResourceNotFoundException($"No ticket found with ID {id}.");
        }

        var comment = await _ticketRepository.AddTicketCommentAsync(id, cmd);
        return comment;
    }
}