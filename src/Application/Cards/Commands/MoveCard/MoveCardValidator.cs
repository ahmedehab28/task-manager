using FluentValidation;

namespace Application.Cards.Commands.MoveCard
{
    public class MoveCardValidator : AbstractValidator<MoveCardCommand>
    {
        public MoveCardValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("CardId is required.");
            RuleFor(x => x.ListId)
                .NotEmpty().WithMessage("ListId is required.");
            RuleFor(cmd => cmd.Position)
              .GreaterThanOrEqualTo(0m).WithMessage("Position must be zero or positive.")
              .PrecisionScale(18, 4, false).WithMessage("Position can have up to 4 decimal places and no more than 18 digits total.");

        }
    }
}
