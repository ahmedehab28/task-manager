using System;
using Domain.Entities.Boards;
using FluentAssertions;
using Xunit;

namespace Domain.Tests.Entities.Boards
{
    public class BoardTests
    {
        [Fact]
        public void Constructor_WithValidTitle_ShouldInitializeProperties()
        {
            // Arrange
            var validTitle = "Project Board";
            var description = "Managing tasks and projects";

            // Act
            var board = new Board(validTitle, description);

            // Assert
            board.Id.Should().NotBe(Guid.Empty, "the board should be assigned a new unique identifier on creation");
            board.Title.Should().Be(validTitle, "the title should be correctly assigned from the constructor");
            board.Description.Should().Be(description, "the description should be set correctly when provided");

            // Allow a small time difference when comparing creation times
            board.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2), "CreatedAt should be set to the current time");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_WithInvalidTitle_ShouldThrowArgumentException(string? invalidTitle)
        {
            // Act
            Action act = () =>
            {
                Board board = new(invalidTitle!);
            };

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Title is required*");
        }
    }
}
