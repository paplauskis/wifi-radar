namespace API.Domain.Models;

public abstract class TimeStampedEntity : BaseEntity
{
    public DateTime CreatedAt { get; init; }

    protected TimeStampedEntity()
    {
        CreatedAt = DateTime.UtcNow;
    }
}