using TaskSync.Domain.Shared;
using TaskSync.Domain.User;

namespace TaskSync.Domain.Ticket.DeleteTicketComment;

public class DeleteTicketCommentCommandHandler : ICommandHandler
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteTicketCommentCommandHandler(ICurrentUserService currentUserService, ITicketRepository ticketRepository)
    {
        _currentUserService = currentUserService;
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<bool>> HandleAsync(int commentId)
    {
        var comment = await _ticketRepository.GetTicketCommentByIdAsync(commentId);
        if (comment == null)
        {
            return Result<bool>.Fail("Could not delete the ticket comment with ID " + commentId + ". Not found.");
        }
        
        // if the current user is author of the comment, allow deletion
        var authorId = comment.CreatedById;
        var currentUser = await _currentUserService.GetCurrentUserAsync();
        if (currentUser?.Id == authorId)
        {
            var result = await _ticketRepository.DeleteTicketCommentAsync(commentId);
            return result ? Result<bool>.Ok(result) : Result<bool>.Fail("Could not delete the ticket comment with ID " + commentId);
        }
        
        return Result<bool>.Fail("Cannot delete comment with ID " + commentId + ". No permissions.");
    }
}