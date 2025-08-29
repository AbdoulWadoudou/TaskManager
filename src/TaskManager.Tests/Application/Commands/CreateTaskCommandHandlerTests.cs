using FluentAssertions;
using Moq;
using TaskManager.Application.Commands.CreateTask;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Repositories;

namespace TaskManager.Tests.Application.Commands;

public class CreateTaskCommandHandlerTests
{
    private readonly Mock<ITaskRepository> _mockRepository;
    private readonly CreateTaskCommandHandler _handler;

    public CreateTaskCommandHandlerTests()
    {
        _mockRepository = new Mock<ITaskRepository>();
        _handler = new CreateTaskCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateTaskAndReturnDto()
    {
        // Arrange
        var command = new CreateTaskCommand(
            "Test Task",
            "Test Description",
            Priority.High,
            DateTime.UtcNow.AddDays(1)
        );

        _mockRepository.Setup(x => x.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()))
                      .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(command.Title);
        result.Description.Should().Be(command.Description);
        result.Priority.Should().Be(command.Priority);
        result.Status.Should().Be(TaskManager.Domain.Enums.TaskStatus.Pending);

        _mockRepository.Verify(x => x.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}