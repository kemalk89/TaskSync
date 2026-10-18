namespace TaskSync.Infrastructure.Entities;

public abstract class AuditedEntity : IAuditedEntity
{
    public string CreatedBy { get; set; } = string.Empty;
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }
}
