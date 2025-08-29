using MediatR;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Queries.GetAllTasks;

public record GetAllTasksQuery : IRequest<IEnumerable<TaskDto>>;