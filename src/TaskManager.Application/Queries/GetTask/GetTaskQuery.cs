using MediatR;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Queries.GetTask;

public record GetTaskQuery(Guid Id) : IRequest<TaskDto>;