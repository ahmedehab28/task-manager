using Application.Boards.Commands.UpdateBoard;
using Domain.Rules;
using FluentValidation.TestHelper;

namespace Application.Tests.Boards.Commands.UpdateBoard
{
    public class UpdateBoardValidatorTests
    {
        private readonly UpdateBoardValidator _validator = new();

        [Theory]
        [InlineData("1")]
        [InlineData("T")]
        [InlineData("#")]
        [InlineData("T1#")]
        [InlineData("Board Title")]
        public void Validator_Should_PassValidation_When_TitleIsValid(string title)
        {
            // Arrange
            var cmd = new UpdateBoardCommand(Guid.NewGuid(), title, "Description");

            // Act
            var result = _validator.TestValidate(cmd);

            // Assertion
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_Should_PassValidation_When_NonKeyBoardFieldsAreNull()
        {
            // Arrange
            var cmd = new UpdateBoardCommand(Guid.NewGuid(), null, null);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_BoardIdIsEmpty()
        {
            // Arrange
            var cmd = new UpdateBoardCommand(Guid.Empty, "Title", "Description");

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.BoardId);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        public void Validtaor_Should_HaveValidationErrors_When_TitleIsInvalid(string? title)
        {
            // Arrange
            var cmd = new UpdateBoardCommand(Guid.NewGuid(), title!, "Description");

            // Act
            var result = _validator.TestValidate(cmd);

            // Assertion
            result.ShouldHaveValidationErrorFor(x => x.Title);

        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_TitleIsTooLong()
        {
            // Arrange
            var cmd = new UpdateBoardCommand(Guid.NewGuid(), new string('A', BoardRules.TitleMaxLength + 1), null);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(b => b.Title);
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_DescriptionIsTooLong()
        {
            // Arrange
            var cmd = new UpdateBoardCommand(Guid.NewGuid(), "Title", new string('A', BoardRules.DescriptionMaxLength + 1));

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(b => b.Description);
        }
    }
}
