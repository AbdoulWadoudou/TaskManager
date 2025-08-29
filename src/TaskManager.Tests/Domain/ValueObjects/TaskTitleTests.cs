using FluentAssertions;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Tests.Domain.ValueObjects;

public class TaskTitleTests
{
    [Fact]
    public void Create_WithValidTitle_ShouldReturnTaskTitle()
    {
        // Arrange
        var titleValue = "Valid Task Title";

        // Act
        var title = TaskTitle.Create(titleValue);

        // Assert
        title.Value.Should().Be(titleValue);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithInvalidTitle_ShouldThrowArgumentException(string? invalidTitle)
    {
        // Act & Assert
        var act = () => TaskTitle.Create(invalidTitle!);
        act.Should().Throw<ArgumentException>()
           .WithMessage("Task title cannot be empty*");
    }

    [Fact]
    public void Create_WithTitleTooLong_ShouldThrowArgumentException()
    {
        // Arrange
        var longTitle = new string('a', 201);

        // Act & Assert
        var act = () => TaskTitle.Create(longTitle);
        act.Should().Throw<ArgumentException>()
           .WithMessage("Task title cannot exceed 200 characters*");
    }

    [Fact]
    public void Create_WithWhitespace_ShouldTrimTitle()
    {
        // Arrange
        var titleWithWhitespace = "  Valid Title  ";

        // Act
        var title = TaskTitle.Create(titleWithWhitespace);

        // Assert
        title.Value.Should().Be("Valid Title");
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var title1 = TaskTitle.Create("Same Title");
        var title2 = TaskTitle.Create("Same Title");

        // Act & Assert
        title1.Equals(title2).Should().BeTrue();
        (title1 == title2).Should().BeFalse(); // Reference equality
    }
}