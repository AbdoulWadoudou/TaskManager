using MediatR;
using TaskManager.Application.Common.Exceptions;
using TaskManager.Domain.Repositories;

namespace TaskManager.Application.Commands.DeleteTask;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
{
    private readonly ITaskRepository _taskRepository;

    public DeleteTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var exists = await _taskRepository.ExistsAsync(request.Id, cancellationToken);
        if (!exists)
            throw new NotFoundException($"Task with id {request.Id} not found");

        await _taskRepository.DeleteAsync(request.Id, cancellationToken);
    }
}