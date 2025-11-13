using Application.Cards.Commands.CreateCard;
using FluentValidation.TestHelper;
using System.Globalization;

namespace Application.Tests.Cards.Commands.CreateCard
{
    public class CreateCardValidatorTests
    {
        private readonly CreateCardValidator _validator = new();

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_ListIdIsInvalid()
        {
            // Arrange
            var cmd = new CreateCardCommand(Guid.Empty, "Title", 1000);

            // Act
            var result = _validator.TestValidate(cmd);

            // Result
            result.ShouldHaveValidationErrorFor(x => x.ListId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Validator_Should_HaveValidationErrors_When_TitleIsInvalid(string? title)
        {
            // Arrange
            var cmd = new CreateCardCommand(Guid.NewGuid(), title!, 1000);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(15.12345)]
        [InlineData(1234567891234567891)]
        [InlineData(123456789123456789.1)]
        public void Validator_Should_Have_ValidationErrors_When_PositionIsInvalid(decimal position)
        {
            // Arrange
            var cmd = new CreateCardCommand(Guid.NewGuid(), "Title", position);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Position);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("T")]
        [InlineData("#")]
        [InlineData("T1#")]
        [InlineData("Card Title")]
        public void Validator_Should_PassValidation_When_TitleIsValid(string title)
        {
            // Arrange
            var cmd = new CreateCardCommand(Guid.NewGuid(), title, 1000);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assertion
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("0")]
        [InlineData("123")]
        [InlineData("12345678912345")]
        [InlineData("1234567891234.1")]
        [InlineData("99999999999999.9999")]
        [InlineData("170000000.46")]
        public void Validator_Should_PassValidation_When_PositionIsValid(string positionStr)
        {
            // Arrange
            var position = decimal.Parse(positionStr, CultureInfo.InvariantCulture);
            var cmd = new CreateCardCommand(Guid.NewGuid(), "Title", position);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_Should_PassValidation_When_InputIsValid()
        {
            // Arrange
            var cmd = new CreateCardCommand(Guid.NewGuid(), "Title", 1000);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
