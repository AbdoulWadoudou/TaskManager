namespace TaskManager.Domain.Common;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}

public interface INotification
{
}