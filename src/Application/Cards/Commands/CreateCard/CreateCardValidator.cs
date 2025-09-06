
using FluentValidation;

namespace Application.Cards.Commands.CreateCard
{
    public class CreateCardValidator : AbstractValidator<CreateCardCommand>
    {
        public CreateCardValidator() 
        {
            RuleFor(x => x.ListId)
                .NotEmpty().WithMessage("List Id is required.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(50).WithMessage("Title cannot exceed 50 characters.");

            RuleFor(cmd => cmd.Position)
              .GreaterThanOrEqualTo(0m).WithMessage("Position must be zero or positive.")
              .PrecisionScale(18, 4, false).WithMessage("Position can have up to 4 decimal places and no more than 18 digits total.");
        }
    }
}
