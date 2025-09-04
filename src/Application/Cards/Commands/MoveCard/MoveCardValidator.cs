using FluentValidation;

namespace Application.Cards.Commands.MoveCard
{
    public class MoveCardValidator : AbstractValidator<MoveCardCommand>
    {
        public MoveCardValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("ProjectId is required.");
            RuleFor(x => x.BoardId)
                .NotEmpty().WithMessage("BoardId is required.");
            RuleFor(x => x.CardId)
                .NotEmpty().WithMessage("CardId is required.");
            RuleFor(x => x.PrevCardId)
                .NotEqual(x => x.CardId).WithMessage("PrevCardId cannot be the same as CardId.");
            RuleFor(x => x.NextCardId)
                .NotEqual(x => x.CardId).WithMessage("NextCardId cannot be the same as CardId.");
            RuleFor(x => x.PrevCardId)
                .NotEqual(x => x.NextCardId).WithMessage("PrevCardId cannot be the same as NextCardId.");
        }
    }
}
