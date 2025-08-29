using TaskManager.Domain.Enums;
using TaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.API.Controllers;

public class UpdateTaskStatusRequest
{
    public TaskStatus Status { get; set; }
}