using MediatR;
using System;

namespace TaskManager.Domain.Events
{
    public class TaskCompletedEvent : INotification // Add this interface
    {
        public Guid TaskId { get; }
        public string Title { get; }
        public DateTime OccurredOn { get; }

        public TaskCompletedEvent(Guid taskId, string title, DateTime occurredOn)
        {
            TaskId = taskId;
            Title = title;
            OccurredOn = occurredOn;
        }
    }
}