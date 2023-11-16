using TaskSync.Domain.Ticket;
using TaskSync.Domain.User;

namespace TaskSync.Controllers.Response;

public class TicketResponse
{

    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public User? Assignee { get; set; }

    public User? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }
    public TicketStatusResponse? Status { get; set; }
    public ProjectResponse Project { get; set; }

    public TicketResponse(TicketModel ticket)
    {
        Id = ticket.Id;
        Title = ticket.Title;
        Description = ticket.Description;
        Assignee = ticket.Assignee;
        CreatedBy = ticket.CreatedBy;
        CreatedDate = ticket.CreatedDate;
        ModifiedDate = ticket.ModifiedDate;
        Status = ticket.Status != null ? new TicketStatusResponse(ticket.Status) : null;
        Project = new ProjectResponse(ticket.Project);
    }
}
