using FluentAssertions;
using FluentValidation;
using MediatR;

namespace Application.Common.Behaviors
{
    public class DummyCommand : IRequest<string>
    {
        public string? Name { get; set; }
    }

    public class DummyValidator : AbstractValidator<DummyCommand>
    {
        public DummyValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required");
        }
    }

    public class ValidationBehaviorTests
    {
        [Fact]
        public void ValidationBehavior_WithInvalidInput_ShouldThrowValidationException()
        {
            // Arrange
            var cmd = new DummyCommand() { Name = "" };
            var validator = new DummyValidator();
            var validators = new List<IValidator<DummyCommand>>() { validator };
            var ValidationBehavior = new ValidationBehavior<DummyCommand, string>(validators);

            RequestHandlerDelegate<string> next = _ => Task.FromResult("Success");

            // Act
            Func<Task> act = async () =>
            {
                await ValidationBehavior.Handle(cmd, next, CancellationToken.None);
            };

            // Assertion
            act.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task ValidationBehavior_WithValidInput_ShouldCallNext()
        {
            // Arrange
            var cmd = new DummyCommand() { Name = "Valid Name" };
            var validator = new DummyValidator();
            var validators = new List<IValidator<DummyCommand>>() { validator };
            var ValidationBehavior = new ValidationBehavior<DummyCommand, string>(validators);

            RequestHandlerDelegate<string> next = _ => Task.FromResult("Success");

            // Act
            var result = await ValidationBehavior.Handle(cmd, next, CancellationToken.None);

            // Assertion
            result.Should().Be("Success");


        }
    }
    
}
