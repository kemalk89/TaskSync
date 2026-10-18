namespace TaskSync.Domain.Ticket;

public class TicketCommentModel
{
    public int Id { get; set; }

    public int TicketId { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public string Comment { get; set; } = string.Empty;

    public string CreatedById { get; set; } = string.Empty;
    
    public User.User? CreatedBy { get; set; }

    public DateTimeOffset CreatedDate { get; set; }
}