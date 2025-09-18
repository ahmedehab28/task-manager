using Application.Cards.Queries.GetBoardWorksoace;
using FluentValidation.TestHelper;

namespace Application.Tests.Boards.Queries.GetBoardWorkspace
{
    public class GetBoardWorkspaceValidatorTests
    {
        private readonly GetBoardWorkspaceValidator _validator = new();

        [Fact]
        public void Validator_Should_PassValidation_When_BoardIdIsValid()
        {
            // Arrange
            var query = new GetBoardWorkspaceQuery(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_BoardIdIsInvalid()
        {
            // Arrange
            var query = new GetBoardWorkspaceQuery(Guid.Empty);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(b => b.BoardId);
        }
    }
}
