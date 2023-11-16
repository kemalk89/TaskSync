namespace TaskSync.Domain.Ticket;

public class TicketCommentModel
{
    public int Id { get; set; }

    public int TicketId { get; set; }
    
    public string Comment { get; set; }
    
    public User.User? CreatedBy { get; set; }

    public DateTimeOffset CreatedDate { get; set; }
}