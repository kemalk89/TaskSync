using TaskSync.Domain.Exceptions;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket.Command;

namespace TaskSync.Domain.Ticket.AddTicketComment;

public class AddTicketCommentCommandHandler : ICommandHandler
{
    private readonly ITicketRepository _ticketRepository;
    private readonly AddTicketCommentCommandValidator _validator;

    public AddTicketCommentCommandHandler(
        ITicketRepository ticketRepository, AddTicketCommentCommandValidator validator)
    {
        _ticketRepository = ticketRepository;
        _validator = validator;
    }

    public async Task<Result<TicketCommentModel>> HandleAsync(int id, AddTicketCommentCommand cmd)
    {
        var validationResult = await _validator.ValidateAsync(cmd);
        if (!validationResult.IsValid)
        {
            return Result<TicketCommentModel>.Fail(
                ResultCodes.ResultCodeValidationFailed, validationResult);
        }
        
        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket == null)
        {
            return Result<TicketCommentModel>.Fail(
                ResultCodes.ResultCodeResourceNotFound, $"No ticket found with ID {id}.");
        }

        var comment = await _ticketRepository.AddTicketCommentAsync(id, cmd);
        return Result<TicketCommentModel>.Ok(comment);
    }
}