using FluentAssertions;
using TaskManager.Application.Commands.CreateTask;
using TaskManager.Domain.Enums;

namespace TaskManager.Tests.Application.Validators;

public class CreateTaskCommandValidatorTests
{
    private readonly CreateTaskCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateTaskCommand(
            "Valid Task Title",
            "Valid Description",
            Priority.Medium,
            DateTime.UtcNow.AddDays(1)
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Validate_WithInvalidTitle_ShouldHaveValidationError(string invalidTitle)
    {
        // Arrange
        var command = new CreateTaskCommand(
            invalidTitle,
            "Valid Description",
            Priority.Medium
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateTaskCommand.Title));
    }

    [Fact]
    public void Validate_WithTitleTooLong_ShouldHaveValidationError()
    {
        // Arrange
        var longTitle = new string('a', 201);
        var command = new CreateTaskCommand(
            longTitle,
            "Valid Description",
            Priority.Medium
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateTaskCommand.Title));
    }

    [Fact]
    public void Validate_WithPastDueDate_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateTaskCommand(
            "Valid Title",
            "Valid Description",
            Priority.Medium,
            DateTime.UtcNow.AddDays(-1)
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateTaskCommand.DueDate));
    }
}