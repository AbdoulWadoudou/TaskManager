using TaskManager.Domain.Enums;

namespace TaskManager.API.Controllers;

public class UpdateTaskRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Priority Priority { get; set; }
    public DateTime? DueDate { get; set; }
}