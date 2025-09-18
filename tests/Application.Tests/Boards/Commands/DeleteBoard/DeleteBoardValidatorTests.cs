using Application.Boards.Commands.DeleteBoard;
using FluentValidation.TestHelper;

namespace Application.Tests.Boards.Commands.DeleteBoard
{
    public class DeleteBoardValidatorTests
    {
        private readonly DeleteBoardValidator _validator = new();

        [Fact]
        public void Validator_Should_PassValidation_When_BoardIdIsValid()
        {
            // Arrange
            var cmd = new DeleteBoardCommand(Guid.NewGuid());

            //
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_BoardIdIsInvalid()
        {
            // Arrange
            var cmd = new DeleteBoardCommand(Guid.Empty);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(b => b.BoardId);
        }
    }
}
