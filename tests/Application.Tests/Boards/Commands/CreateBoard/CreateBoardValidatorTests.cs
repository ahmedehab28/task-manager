using Application.Boards.Commands.CreateBoard;
using Domain.Rules;
using FluentValidation.TestHelper;

namespace Application.Tests.Boards.Commands.CreateBoard
{
    public class CreateBoardValidatorTests
    {
        private readonly CreateBoardValidator _validator = new();

        [Theory]
        [InlineData("1")]
        [InlineData("T")]
        [InlineData("#")]
        [InlineData("T1#")]
        [InlineData("Board Title")]
        public void Validator_Should_PassValidation_When_TitleIsValid(string title)
        {
            // Arrange
            var cmd = new CreateBoardCommand(Guid.NewGuid(), title, "Description");

            // Act
            var result = _validator.TestValidate(cmd);

            // Assertion
            result.ShouldNotHaveValidationErrorFor(b => b.Title);
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_ProjectIdIsEmpty()
        {
            // Arrange
            var cmd = new CreateBoardCommand(Guid.Empty, "Valid title", null);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.ProjectId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void CreateBoardValidtaor_Should_HaveValidationErrors_When_TitleIsInvalid(string? title)
        {
            // Arrange
            var cmd = new CreateBoardCommand(Guid.NewGuid(),title!, "Board Description");

            // Act
            var result = _validator.TestValidate(cmd);

            // Assertion
            result.ShouldHaveValidationErrorFor(x => x.Title);

        }

        

        [Fact]
        public void CreateBoardValidator_Should_HaveValidationErrors_When_TitleIsTooLong()
        {
            // Arrange
            var cmd = new CreateBoardCommand(Guid.NewGuid(), new string('A', BoardRules.TitleMaxLength + 1), null);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(b => b.Title);
        }

        [Fact]
        public void CreateBoardValidator_Should_HaveValidationErrors_When_DescriptionIsTooLong()
        {
            // Arrange
            var cmd = new CreateBoardCommand(Guid.NewGuid(), "Title", new string('A', BoardRules.DescriptionMaxLength + 1));

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(b => b.Description);
        }
    }
}
