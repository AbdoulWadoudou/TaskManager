using MediatR;
using TaskManager.Application.Common.Exceptions;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Repositories;

namespace TaskManager.Application.Commands.UpdateTaskStatus;

public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand, TaskDto>
{
    private readonly ITaskRepository _taskRepository;

    public UpdateTaskStatusCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskDto> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Task with id {request.Id} not found");

        switch (request.Status)
        {
            case Domain.Enums.TaskStatus.InProgress:
                task.MarkAsInProgress();
                break;
            case Domain.Enums.TaskStatus.Completed:
                task.MarkAsCompleted();
                break;
            case Domain.Enums.TaskStatus.Cancelled:
                task.MarkAsCancelled();
                break;
            default:
                throw new InvalidOperationException($"Cannot update task status to {request.Status}");
        }

        await _taskRepository.UpdateAsync(task, cancellationToken);

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