using Application.Boards.Queries.GetAllBoards;
using FluentValidation.TestHelper;

namespace Application.Tests.Boards.Queries.GetAllBoards
{
    public class GetAllBoardsValidatorTests
    {
        private readonly GetAllBoardsValidator _validator = new();

        [Fact]
        public void Validator_Should_PassValidation_When_ProjectIdIsValid() 
        {
            // Arrange
            var cmd = new GetAllBoardsQuery(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_ProjectIdIsInvalid()
        {
            // Arrange
            var cmd = new GetAllBoardsQuery(Guid.Empty);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ProjectId);
        }
    }
}
