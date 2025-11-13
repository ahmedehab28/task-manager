using Application.Cards.Commands.UpdateCard;
using Domain.Rules;
using FluentValidation.TestHelper;

namespace Application.Tests.Cards.Commands.UpdateCard
{
    public class UpdateCardValidatorTests
    {
        private readonly UpdateCardValidator _validator = new();

        [Fact]
        public void Validator_Should_PassValidation_When_CardIdIsValid()
        {
            // Arrange
            var cmd = new UpdateCardCommand(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_CardIdIsInvalid()
        {
            // Arrange
            var cmd = new UpdateCardCommand(Guid.Empty);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(b => b.CardId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Validator_Should_HaveValidationErrors_When_TitleIsEmpty(string? title)
        {
            // Arrange
            var cmd = new UpdateCardCommand(Guid.NewGuid(), title);

            // Act
            var result = _validator.TestValidate(cmd);
            // Assert
            result.ShouldHaveValidationErrorFor(b => b.Title.Value)
                .WithErrorMessage(CardRules.TitleRequiredErrorMessage);
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_TitleExceedsMaxLength()
        {
            // Arrange
            var longTitle = new string('a', CardRules.TitleMaxLength + 1);
            var cmd = new UpdateCardCommand(Guid.NewGuid(), longTitle);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(b => b.Title.Value)
                .WithErrorMessage(CardRules.TitleMaxLengthErrorMessage);
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
            var cmd = new UpdateCardCommand(Guid.NewGuid(), title);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_DescriptionExceedsMaxLength()
        {
            // Arrange
            var longDescription = new string('a', CardRules.DescriptionMaxLength + 1);
            var cmd = new UpdateCardCommand(Guid.NewGuid(), Description: longDescription);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(b => b.Description.Value)
                .WithErrorMessage(CardRules.DescriptionMaxLengthErrorMessage);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("D")]
        [InlineData("This is a valid description.")]
        public void Validator_Should_PassValidation_When_DescriptionIsValid(string? description)
        {
            // Arrange
            var cmd = new UpdateCardCommand(Guid.NewGuid(), Description: description);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_DueAtIsInThePast()
        {
            // Arrange
            var pastDate = DateTime.UtcNow.AddMinutes(-10);
            var cmd = new UpdateCardCommand(Guid.NewGuid(), DueAt: pastDate);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(b => b.DueAt.Value)
                .WithErrorMessage(CardRules.DueDateInPastErrorMessage);
        }

        public static IEnumerable<object[]> ValidDueDates =>
            new List<object[]>
            {
                new object[] { null },
                new object[] { DateTime.UtcNow.AddMinutes(1) },
                new object[] { DateTime.UtcNow.AddDays(1) }
            };
        [Theory]
        [MemberData(nameof(ValidDueDates))]
        public void Validator_Should_PassValidation_When_DueAtIsValid(DateTime? dueAt)
        {
            // Arrange
            var cmd = new UpdateCardCommand(Guid.NewGuid(), DueAt: dueAt);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_PositionIsNull()
        {
            // Arrange
            var cmd = new UpdateCardCommand(Guid.NewGuid(), Position: null);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(b => b.Position.Value)
                .WithErrorMessage(CardRules.PositionRequiredErrorMessage);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("-1")]
        [InlineData("-100.5")]
        public void Validator_Should_HaveValidationErrors_When_PositionIsNegativeOrZero(string positionStr)
        {
            // Arrange
            var position = decimal.Parse(positionStr);
            var cmd = new UpdateCardCommand(Guid.NewGuid(), Position: position);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(b => b.Position.Value)
                .WithErrorMessage(CardRules.PositionNonNegativeErrorMessage);
        }

        [Theory]
        [InlineData("15.12345")]
        [InlineData("123456789123456")]
        [InlineData("123456789123456.1")]
        public void Validator_Should_HaveValidationErrors_When_PositionExceedsPrecisionOrScale(string positionStr)
        {
            // Arrange
            var position = decimal.Parse(positionStr);
            var cmd = new UpdateCardCommand(Guid.NewGuid(), Position: position);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(b => b.Position.Value)
                .WithErrorMessage(CardRules.PositionPrecisionErrorMessage);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("12345678912345")]
        [InlineData("1234567891234.1")]
        [InlineData("99999999999999.9999")]
        [InlineData("170000000.46")]
        public void Validator_Should_PassValidation_When_PositionIsValid(string positionStr)
        {
            // Arrange
            var position = decimal.Parse(positionStr);
            var cmd = new UpdateCardCommand(Guid.NewGuid(), Position: position);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        public static IEnumerable<object[]> EmptyGuid =>
        new[]
        {
            new object[] { (Guid?)null },
            new object[] { Guid.Empty }
        };
        [Theory]
        [MemberData(nameof(EmptyGuid))]
        public void Validator_Should_HaveValidationErrors_When_TargetListIdIsInvalid(Guid? targetListId)
        {
            // Arrange
            var cmd = new UpdateCardCommand(Guid.NewGuid(), TargetListId: targetListId, Position: 1000m);
            // Act
            var result = _validator.TestValidate(cmd);
            // Assert
            result.ShouldHaveValidationErrorFor(b => b.TargetListId)
                .WithErrorMessage(CardRules.TargetListIdInvalidErrorMessage);
        }

        [Fact]
        public void Validator_Should_PassValidation_When_TargetListIdIsValid()
        {
            // Arrange
            var cmd = new UpdateCardCommand(Guid.NewGuid(), TargetListId: Guid.NewGuid(), Position: 1000m);

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_Should_HaveValidationErrors_When_PositionNotProvidedWithTargetListId()
        {
            // Arrange
            var cmd = new UpdateCardCommand(Guid.NewGuid(), TargetListId: Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(cmd);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Position)
                .WithErrorMessage(CardRules.MoveCardRequiresPositionErrorMessage);
        }
    }
}
