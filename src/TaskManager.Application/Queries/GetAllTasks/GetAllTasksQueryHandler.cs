using MediatR;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Repositories;

namespace TaskManager.Application.Queries.GetAllTasks;

public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskDto>>
{
    private readonly ITaskRepository _taskRepository;

    public GetAllTasksQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IEnumerable<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository.GetAllAsync(cancellationToken);

        return tasks.Select(task => new TaskDto
        {
            Id = task.Id,
            Title = task.Title.Value,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        });
    }
}