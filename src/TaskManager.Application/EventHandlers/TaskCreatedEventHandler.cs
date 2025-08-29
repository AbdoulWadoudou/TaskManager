using MediatR;
using System;

namespace TaskManager.Domain.Events
{
    public class TaskCreatedEvent : INotification // Ajouter cette interface
    {
        public Guid TaskId { get; }
        public string Title { get; }
        public DateTime OccurredOn { get; }

        public TaskCreatedEvent(Guid taskId, string title, DateTime occurredOn)
        {
            TaskId = taskId;
            Title = title;
            OccurredOn = occurredOn;
        }
    }
}