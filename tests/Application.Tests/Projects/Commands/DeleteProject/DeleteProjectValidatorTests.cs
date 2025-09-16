using Application.Common.Interfaces;
using Application.Projects.Commands.CreateProject;
using Application.Projects.Commands.DeleteProject;
using FluentValidation.TestHelper;
using Moq;

namespace Application.Tests.Projects.Commands.DeleteProject
{
    public class DeleteProjectValidatorTests
    {
        private readonly DeleteProjectValidator _validator = new();
        [Fact]
        public void Validator_Should_PassValidation_When_ProjectIdIsValid()
        {
            // Arrange
            var cmd = new DeleteProjectCommand(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_ProjectIdIsInvalid()
        {
            // Arrange
            var cmd = new DeleteProjectCommand(Guid.Empty);

            // Assert
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ProjectId);
        }
    }
}
