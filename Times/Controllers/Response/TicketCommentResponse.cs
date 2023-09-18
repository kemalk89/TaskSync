using Times.Domain.Ticket;

namespace Times.Controllers.Response;

public class TicketCommentResponse
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public string Comment { get; set; }
    
    public UserResponse? CreatedBy { get; set; }
    
    public DateTimeOffset CreatedDate { get; set; }
    
    public TicketCommentResponse(TicketCommentModel model)
    {
        Id = model.Id;
        TicketId = model.TicketId;
        Comment = model.Comment;
        CreatedBy = new UserResponse(model.CreatedBy);
        CreatedDate = model.CreatedDate;
    }
}