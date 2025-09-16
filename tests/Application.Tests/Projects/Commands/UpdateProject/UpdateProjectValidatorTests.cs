using Application.Projects.Commands.UpdateProject;
using Domain.Rules;
using FluentValidation.TestHelper;

namespace Application.Tests.Projects.Commands.UpdateProject
{
    public class UpdateProjectValidatorTests
    {
        private readonly UpdateProjectValidator _validator = new();

        [Theory]
        [InlineData("1")]
        [InlineData("T")]
        [InlineData("#")]
        [InlineData("T1#")]
        [InlineData("Project Title")]
        public void Validator_Should_PassValidation_When_TitleIsValid(string title)
        {
            // Arrange
            var cmd = new UpdateProjectCommand(Guid.NewGuid(), title, "Description");

            // Act
            var result = _validator.TestValidate(cmd);

            // Assertion
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_Should_PassValidation_When_NonKeyProjectFieldsAreNull() 
        {
            // Arrange
            var cmd = new UpdateProjectCommand(Guid.NewGuid(), null, null);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_ProjectIdIsEmpty()
        {
            // Arrange
            var cmd = new UpdateProjectCommand(Guid.Empty, "Title", "Description");

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.ProjectId);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        public void Validtaor_Should_HaveValidationErrors_When_TitleIsInvalid(string? title)
        {
            // Arrange
            var cmd = new UpdateProjectCommand(Guid.NewGuid(), title!, "Description");

            // Act
            var result = _validator.TestValidate(cmd);

            // Assertion
            result.ShouldHaveValidationErrorFor(x => x.Title);

        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_TitleIsTooLong()
        {
            // Arrange
            var cmd = new UpdateProjectCommand(Guid.NewGuid(), new string('A', ProjectRules.TitleMaxLength + 1), null);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(b => b.Title);
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_DescriptionIsTooLong()
        {
            // Arrange
            var cmd = new UpdateProjectCommand(Guid.NewGuid(), "Title", new string('A', ProjectRules.DescriptionMaxLength + 1));

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(b => b.Description);
        }
    }
}
