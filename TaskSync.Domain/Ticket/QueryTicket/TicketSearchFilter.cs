namespace TaskSync.Domain.Ticket.QueryTicket;

public class TicketSearchFilter
{
    public string SearchText { get; set; } = string.Empty;
    public List<int> StatusIds { get; set; } = [];
    public List<int> ProjectIds { get; set; } = [];
    public List<int> AssigneeIds { get; set; } = [];
}