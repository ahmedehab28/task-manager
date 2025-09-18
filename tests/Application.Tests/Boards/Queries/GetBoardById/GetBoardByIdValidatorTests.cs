using Application.Boards.Queries.GetBoardById;
using FluentValidation.TestHelper;

namespace Application.Tests.Boards.Queries.GetBoardById
{
    public class GetBoardByIdValidatorTests
    {
        private readonly GetBoardByIdValidator _validator = new();

        [Fact]
        public void Validator_Should_PassValidation_When_BoardIdIsValid()
        {
            // Arrange
            var query = new GetBoardByIdQuery(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_WhenBoardIdIsInvalid()
        {
            // Arrange
            var query = new GetBoardByIdQuery(Guid.Empty);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(b => b.BoardId);
        }
    }
}
