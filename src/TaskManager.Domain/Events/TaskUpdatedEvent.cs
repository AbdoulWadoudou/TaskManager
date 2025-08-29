using TaskManager.Domain.Common;

namespace TaskManager.Domain.Events;

public record TaskUpdatedEvent(Guid TaskId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}