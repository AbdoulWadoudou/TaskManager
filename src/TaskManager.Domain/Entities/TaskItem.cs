using TaskManager.Domain.Common;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Events;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Domain.Entities;

public class TaskItem : BaseEntity
{
    public TaskTitle Title { get; private set; }
    public string? Description { get; private set; }
    public Enums.TaskStatus Status { get; private set; }
    public Priority Priority { get; private set; }
    public DateTime? DueDate { get; private set; }

    // Constructeur privé pour EF Core
    private TaskItem() { }

    public TaskItem(TaskTitle title, string? description, Priority priority, DateTime? dueDate = null)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description;
        Status = Enums.TaskStatus.Pending;
        Priority = priority;
        DueDate = dueDate;

        AddDomainEvent(new TaskCreatedEvent(Id, Title.Value));
    }

    public void UpdateTitle(TaskTitle newTitle)
    {
        if (newTitle == null)
            throw new ArgumentNullException(nameof(newTitle));

        Title = newTitle;
        MarkAsUpdated();
        AddDomainEvent(new TaskUpdatedEvent(Id));
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
        MarkAsUpdated();
        AddDomainEvent(new TaskUpdatedEvent(Id));
    }

    public void UpdatePriority(Priority priority)
    {
        Priority = priority;
        MarkAsUpdated();
        AddDomainEvent(new TaskUpdatedEvent(Id));
    }

    public void UpdateDueDate(DateTime? dueDate)
    {
        DueDate = dueDate;
        MarkAsUpdated();
        AddDomainEvent(new TaskUpdatedEvent(Id));
    }

    public void MarkAsInProgress()
    {
        if (Status == Enums.TaskStatus.Completed || Status == Enums.TaskStatus.Cancelled)
            throw new InvalidOperationException("Cannot mark completed or cancelled task as in progress");

        Status = Enums.TaskStatus.InProgress;
        MarkAsUpdated();
        AddDomainEvent(new TaskStatusChangedEvent(Id, Enums.TaskStatus.InProgress));
    }

    public void MarkAsCompleted()
    {
        if (Status == Enums.TaskStatus.Cancelled)
            throw new InvalidOperationException("Cannot mark cancelled task as completed");

        Status = Enums.TaskStatus.Completed;
        MarkAsUpdated();
        AddDomainEvent(new TaskStatusChangedEvent(Id, Enums.TaskStatus.Completed));
        AddDomainEvent(new TaskCompletedEvent(Id, Title.Value));
    }

    public void MarkAsCancelled()
    {
        if (Status == Enums.TaskStatus.Completed)
            throw new InvalidOperationException("Cannot cancel completed task");

        Status = Enums.TaskStatus.Cancelled;
        MarkAsUpdated();
        AddDomainEvent(new TaskStatusChangedEvent(Id, Enums.TaskStatus.Cancelled));
    }
}