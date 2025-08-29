using MediatR;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Application.Commands.CreateTask;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
{
    private readonly ITaskRepository _taskRepository;

    public CreateTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var title = TaskTitle.Create(request.Title);
        var task = new TaskItem(title, request.Description, request.Priority, request.DueDate);

        await _taskRepository.AddAsync(task, cancellationToken);

        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title.Value,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }
}