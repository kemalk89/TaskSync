namespace TaskSync.Domain.Shared;

public class PaginationQuery
{
    public int PageNumber { get; }
    public int PageSize { get; }

    public PaginationQuery(int pageNumber = 1, int pageSize = 50)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}