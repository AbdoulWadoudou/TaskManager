using MediatR;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Enums;
using TaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Application.Commands.UpdateTaskStatus;

public record UpdateTaskStatusCommand(Guid Id, TaskStatus Status) : IRequest<TaskDto>;