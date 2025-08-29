using MediatR;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Commands.UpdateTask;

public record UpdateTaskCommand(
    Guid Id,
    string Title,
    string? Description,
    Priority Priority,
    DateTime? DueDate
) : IRequest<TaskDto>;