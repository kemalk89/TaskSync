namespace TaskSync.Domain.Ticket;

public interface ILabelRepository
{
    public Task<TicketLabelModel?> FindByIdAsync(int id);
    public Task<int> CreateLabelAsync(string text);
}