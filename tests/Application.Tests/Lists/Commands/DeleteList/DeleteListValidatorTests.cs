using Application.List.Commands.DeleteList;
using FluentValidation.TestHelper;

namespace Application.Tests.Lists.Commands.DeleteList
{
    public class DeleteListValidatorTests
    {
        private readonly DeleteListValidator _validator = new();

        [Fact]
        public void Validator_Should_PassValidation_When_ListIdIsValid()
        {
            // Arrange
            var cmd = new DeleteListCommand(Guid.NewGuid());

            //
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_ListIdIsInvalid()
        {
            // Arrange
            var cmd = new DeleteListCommand(Guid.Empty);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(b => b.ListId);
        }
    }
}
