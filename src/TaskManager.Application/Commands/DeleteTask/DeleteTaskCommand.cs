using MediatR;

namespace TaskManager.Application.Commands.DeleteTask;

public record DeleteTaskCommand(Guid Id) : IRequest;