using Application.List.Queries.GetListById;
using FluentValidation.TestHelper;

namespace Application.Tests.Lists.Queries.GetListById
{
    public class GetListByIdValidatorTests
    {
        private readonly GetListByIdValidator _validator = new();
        [Fact]
        public void Validator_Should_PassValidation_When_ListIdIsValid()
        {
            // Arrange
            var query = new GetListByIdQuery(Guid.NewGuid());

            //
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_ListIdIsInvalid()
        {
            // Arrange
            var query = new GetListByIdQuery(Guid.Empty);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(b => b.ListId);
        }
    }
}
