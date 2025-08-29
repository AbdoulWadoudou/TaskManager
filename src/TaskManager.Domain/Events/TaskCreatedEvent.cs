using TaskManager.Domain.Common;

namespace TaskManager.Domain.Events;

public record TaskCreatedEvent(Guid TaskId, string Title) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}