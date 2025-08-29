using MediatR;
using TaskManager.Application.Common.Exceptions;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Repositories;

namespace TaskManager.Application.Queries.GetTask;

public class GetTaskQueryHandler : IRequestHandler<GetTaskQuery, TaskDto>
{
    private readonly ITaskRepository _taskRepository;

    public GetTaskQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskDto> Handle(GetTaskQuery request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Task with id {request.Id} not found");

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