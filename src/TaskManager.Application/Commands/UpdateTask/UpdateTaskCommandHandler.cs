using MediatR;
using TaskManager.Application.Common.Exceptions;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Application.Commands.UpdateTask;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskDto>
{
    private readonly ITaskRepository _taskRepository;

    public UpdateTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Task with id {request.Id} not found");

        var title = TaskTitle.Create(request.Title);
        task.UpdateTitle(title);
        task.UpdateDescription(request.Description);
        task.UpdatePriority(request.Priority);
        task.UpdateDueDate(request.DueDate);

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