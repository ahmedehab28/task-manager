using Application.Projects.Queries.GetProjectById;
using FluentValidation.TestHelper;

namespace Application.Tests.Projects.Queries.GetProjectById
{
    public class GetProjectByIdValidatorTests
    {
        private readonly GetProjectByIdValidator _validator = new();

        [Fact]
        public void Validator_Should_PassValidation_When_ProjectIdIsValid()
        {
            // Arrange
            var cmd = new GetProjectByIdQuery(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_ProjectIdIsInvalid()
        {
            // Arrange
            var cmd = new GetProjectByIdQuery(Guid.Empty);

            // Assert
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ProjectId);
        }
    }
}
