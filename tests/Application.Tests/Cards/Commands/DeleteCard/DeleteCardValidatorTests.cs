using Application.Cards.Commands.DeleteCard;
using FluentValidation.TestHelper;

namespace Application.Tests.Cards.Commands.DeleteCard
{
    public class DeleteCardValidatorTests
    {
        private readonly DeleteCardValidator _validator = new();
        [Fact]
        public void Validator_Should_PassValidation_When_CardIdIsValid()
        {
            // Arrange
            var cmd = new DeleteCardCommand(Guid.NewGuid());

            //
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_CardIdIsInvalid()
        {
            // Arrange
            var cmd = new DeleteCardCommand(Guid.Empty);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(b => b.CardId);
        }
    }
}
