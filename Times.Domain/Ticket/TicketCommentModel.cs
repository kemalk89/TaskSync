namespace Times.Domain.Ticket;

using User;

public class TicketCommentModel
{
    public int Id { get; set; }

    public int TicketId { get; set; }
    
    public string Comment { get; set; }
    
    public User? CreatedBy { get; set; }

    public DateTimeOffset CreatedDate { get; set; }
}