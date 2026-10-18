namespace TaskSync.Infrastructure.Entities;

public interface IAuditedEntity
{
    public string CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }
}