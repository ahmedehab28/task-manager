using FluentValidation;

namespace Application.Cards.Commands.UpdateCard
{
    public class UpdateCardDetailsValidator : AbstractValidator<UpdateCardDetailsCommand>
    {
        public UpdateCardDetailsValidator() 
        {
            RuleFor(x => x.CardId).NotEmpty().WithMessage("CardId is required.");
            RuleFor(x => x.Title).MaximumLength(50).NotEmpty().When(x => x.Title != null);
            RuleFor(x => x.Description).MaximumLength(500).When(x => x.Description != null);
            RuleFor(x => x.DueAt).GreaterThan(DateTime.UtcNow).When(x => x.DueAt != null);
        }
    }
}
