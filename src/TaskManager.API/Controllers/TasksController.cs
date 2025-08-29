using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Commands.CreateTask;
using TaskManager.Application.Commands.DeleteTask;
using TaskManager.Application.Commands.UpdateTask;
using TaskManager.Application.Commands.UpdateTaskStatus;
using TaskManager.Application.DTOs;
using TaskManager.Application.Queries.GetAllTasks;
using TaskManager.Application.Queries.GetTask;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Récupère toutes les tâches
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetAllTasks(CancellationToken cancellationToken)
    {
        var tasks = await _mediator.Send(new GetAllTasksQuery(), cancellationToken);
        return Ok(tasks);
    }

    /// <summary>
    /// Récupère une tâche par son ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDto>> GetTask(Guid id, CancellationToken cancellationToken)
    {
        var task = await _mediator.Send(new GetTaskQuery(id), cancellationToken);
        return Ok(task);
    }

    /// <summary>
    /// Crée une nouvelle tâche
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TaskDto>> CreateTask([FromBody] CreateTaskCommand command, CancellationToken cancellationToken)
    {
        var task = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    /// <summary>
    /// Met à jour une tâche existante
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<TaskDto>> UpdateTask(Guid id, [FromBody] UpdateTaskRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateTaskCommand(id, request.Title, request.Description, request.Priority, request.DueDate);
        var task = await _mediator.Send(command, cancellationToken);
        return Ok(task);
    }

    /// <summary>
    /// Met à jour le statut d'une tâche
    /// </summary>
    [HttpPatch("{id}/status")]
    public async Task<ActionResult<TaskDto>> UpdateTaskStatus(Guid id, [FromBody] UpdateTaskStatusRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateTaskStatusCommand(id, request.Status);
        var task = await _mediator.Send(command, cancellationToken);
        return Ok(task);
    }

    /// <summary>
    /// Supprime une tâche
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTask(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteTaskCommand(id), cancellationToken);
        return NoContent();
    }
}