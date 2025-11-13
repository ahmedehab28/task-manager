using Application.Cards.Queries.GetCardById;
using FluentValidation.TestHelper;

namespace Application.Tests.Cards.Queries.GetCardById
{
    public class GetCardByIdValidatorTests
    {
        private readonly GetCardByIdValidator _validator = new();

        [Fact]
        public void Validator_Should_PassValidation_When_CardIdIsValid()
        {
            // Arrange
            var query = new GetCardByIdQuery(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_CardIdIsInvalid()
        {
            // Arrange
            var query = new GetCardByIdQuery(Guid.Empty);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(b => b.CardId);
        }
    }
}
