using FluentAssertions;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Events;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Tests.Domain.Entities;

public class TaskItemTests
{
    [Fact]
    public void Constructor_ShouldCreateTaskWithCorrectProperties()
    {
        // Arrange
        var title = TaskTitle.Create("Test Task");
        var description = "Test Description";
        var priority = Priority.High;
        var dueDate = DateTime.UtcNow.AddDays(1);

        // Act
        var task = new TaskItem(title, description, priority, dueDate);

        // Assert
        task.Id.Should().NotBeEmpty();
        task.Title.Value.Should().Be("Test Task");
        task.Description.Should().Be(description);
        task.Status.Should().Be(TaskManager.Domain.Enums.TaskStatus.Pending);
        task.Priority.Should().Be(priority);
        task.DueDate.Should().Be(dueDate);
        task.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        task.DomainEvents.Should().ContainSingle(e => e is TaskCreatedEvent);
    }

    [Fact]
    public void MarkAsCompleted_ShouldChangeStatusToCompleted()
    {
        // Arrange
        var title = TaskTitle.Create("Test Task");
        var task = new TaskItem(title, "Description", Priority.Medium);

        // Act
        task.MarkAsCompleted();

        // Assert
        task.Status.Should().Be(TaskManager.Domain.Enums.TaskStatus.Completed);
        task.UpdatedAt.Should().NotBeNull();
        task.DomainEvents.Should().Contain(e => e is TaskCompletedEvent);
    }

    [Fact]
    public void MarkAsCompleted_WhenTaskIsCancelled_ShouldThrowException()
    {
        // Arrange
        var title = TaskTitle.Create("Test Task");
        var task = new TaskItem(title, "Description", Priority.Medium);
        task.MarkAsCancelled();

        // Act & Assert
        var act = () => task.MarkAsCompleted();
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("Cannot mark cancelled task as completed");
    }

    [Fact]
    public void UpdateTitle_ShouldUpdateTitleAndMarkAsUpdated()
    {
        // Arrange
        var originalTitle = TaskTitle.Create("Original Title");
        var task = new TaskItem(originalTitle, "Description", Priority.Medium);
        var newTitle = TaskTitle.Create("Updated Title");

        // Act
        task.UpdateTitle(newTitle);

        // Assert
        task.Title.Value.Should().Be("Updated Title");
        task.UpdatedAt.Should().NotBeNull();
        task.DomainEvents.Should().Contain(e => e is TaskUpdatedEvent);
    }
}