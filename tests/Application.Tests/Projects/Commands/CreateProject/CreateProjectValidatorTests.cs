using Application.Projects.Commands.CreateProject;
using Domain.Rules;
using FluentValidation.TestHelper;

namespace Application.Tests.Projects.Commands.CreateProject
{
    public class CreateProjectValidatorTests
    {
        private readonly CreateProjectValidator _validator = new();

        [Theory]
        [InlineData("Valid Title")]
        [InlineData("!")]
        [InlineData("123")]
        public void Validator_Should_PassValidation_When_TitleIsValid(string title)
        {
            // Arrange
            var cmd = new CreateProjectCommand(title, null);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldNotHaveValidationErrorFor(p => p.Title);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Validator_Should_HaveValidationErrors_When_TitleIsInvalid(string? title) 
        {
            // Arrange
            var cmd = new CreateProjectCommand(title!, null);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Title);
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_TitleIsTooLong() 
        {
            // Arrange
            var cmd = new CreateProjectCommand(new string('A',ProjectRules.TitleMaxLength+1), null);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Title);
        }

        [Fact]
        public void CreateProjectValidator_Should_haveValidationErrors_WhenDescriptionIsTooLong()
        {
            // Arrange
            var cmd = new CreateProjectCommand("Title", new string('A', ProjectRules.DescriptionMaxLength + 1));

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Description);
        }
    }
}
