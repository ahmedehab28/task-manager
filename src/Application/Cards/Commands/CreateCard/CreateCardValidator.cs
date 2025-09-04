
using FluentValidation;

namespace Application.Cards.Commands.CreateCard
{
    public class CreateCardValidator : AbstractValidator<CreateCardCommand>
    {
        public CreateCardValidator() 
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(50).WithMessage("Title cannot exceed 50 characters.");
        }
    }
}
