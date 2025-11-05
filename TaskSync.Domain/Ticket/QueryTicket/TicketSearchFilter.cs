namespace TaskSync.Domain.Ticket.QueryTicket;

public class TicketSearchFilter
{
    public string SearchText { get; set; } = string.Empty;
    public List<int> Status { get; set; } = [];
}