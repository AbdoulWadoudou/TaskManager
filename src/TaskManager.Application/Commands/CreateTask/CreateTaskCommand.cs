using MediatR;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Commands.CreateTask;

public record CreateTaskCommand(
    string Title,
    string? Description,
    Priority Priority,
    DateTime? DueDate = null
) : IRequest<TaskDto>;