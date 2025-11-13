using FluentValidation;

namespace Application.Cards.Commands.DeleteCard
{
    public class DeleteCardValidator : AbstractValidator<DeleteCardCommand>
    {
        public DeleteCardValidator() 
        {
            RuleFor(x => x.CardId)
                .NotEmpty().WithMessage("CardId is required.");
        }
    }
}
