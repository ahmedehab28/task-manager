using Application.List.Commands.CreateList;
using Application.List.Commands.UpdateList;
using FluentValidation.TestHelper;
using System.Globalization;
namespace Application.Tests.Lists.Commands.UpdateList
{
    public class UpdateListValidatorTests
    {
        private readonly UpdateListValidator _validator = new();

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_ListIdIsInvalid()
        {
            // Arrange
            var cmd = new UpdateListCommand(Guid.Empty, Guid.NewGuid(), "Title", 1000m);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ListId);
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_BoardIdIsInvalid()
        {
            // Arrange
            var cmd = new UpdateListCommand(Guid.NewGuid(), Guid.Empty, "Title", 1000m);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.BoardId);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Validator_Should_HaveValidationErrors_When_TitleIsNotNullButIsInvalid(string title)
        {
            // Arrange
            var cmd = new UpdateListCommand(Guid.NewGuid(), Guid.NewGuid(), title, 1000m);

            // Act
            var result = _validator.TestValidate(cmd);

            // Result
            result.ShouldHaveValidationErrorFor(x => x.Title);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(15.12345)]
        [InlineData(123456789123456)]
        [InlineData(123456789123456.1)]
        public void Validator_Should_HaveValidationErrors_When_PositionIsInvalid(decimal position)
        {
            // Arrange
            var cmd = new UpdateListCommand(Guid.NewGuid(), Guid.NewGuid(), "Title", position);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Position);
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_PositionNotProvidedWithBoardId()
        {
            // Arrange
            var cmd = new UpdateListCommand(Guid.NewGuid(), Guid.NewGuid(), "Title", null);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Position);
        }

        [Fact]
        public void Validator_Should_PassValidation_When_NonListIdInputsAreNull()
        {
            // Arrange
            var cmd = new UpdateListCommand(Guid.NewGuid(), null, null, null);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("1")]
        [InlineData("T")]
        [InlineData("#")]
        [InlineData("T1#")]
        [InlineData("List Title")]
        public void Validator_Should_PassValidation_When_TitleIsValid(string title)
        {
            // Arrange
            var cmd = new UpdateListCommand(Guid.NewGuid(), Guid.NewGuid(), title, 1000);

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
            var cmd = new UpdateListCommand(Guid.NewGuid(), Guid.NewGuid(), "Title", position);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
