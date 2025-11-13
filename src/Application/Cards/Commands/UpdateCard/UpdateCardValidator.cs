using Domain.Rules;
using FluentValidation;

namespace Application.Cards.Commands.UpdateCard
{
    public class UpdateCardValidator : AbstractValidator<UpdateCardCommand>
    {
        public UpdateCardValidator() 
        {
            RuleFor(x => x.CardId)
                .NotEmpty().WithMessage(CardRules.CardIdInvalidErrorMessage);

            RuleFor(x => x.Title.Value)
                .NotEmpty().WithMessage(CardRules.TitleRequiredErrorMessage)
                .MaximumLength(CardRules.TitleMaxLength).WithMessage(CardRules.TitleMaxLengthErrorMessage)
                .When(x => x.Title.IsAssigned);

            RuleFor(x => x.Description.Value)
                .MaximumLength(500).WithMessage(CardRules.DescriptionMaxLengthErrorMessage)
                .When(x => x.Description.IsAssigned);

            RuleFor(x => x.DueAt.Value)
                .GreaterThan(DateTime.UtcNow).WithMessage(CardRules.DueDateInPastErrorMessage)
                .When(x => x.DueAt.IsAssigned && x.DueAt.Value != null);

            RuleFor(x => x.Position.Value)
                .NotEmpty().WithMessage(CardRules.PositionRequiredErrorMessage)
                .GreaterThan(0m).WithMessage(CardRules.PositionNonNegativeErrorMessage)
                .PrecisionScale(CardRules.PositionPrecision, CardRules.PositionScale, false).WithMessage(CardRules.PositionPrecisionErrorMessage)
                .When(x => x.Position.IsAssigned);

            RuleFor(x => x.TargetListId)
                .Must(t => t.Value != Guid.Empty && t.Value != null)
                .WithMessage(CardRules.TargetListIdInvalidErrorMessage)
                .DependentRules(() =>
                {
                    RuleFor(x => x.Position)
                        .Must(p => p.IsAssigned)
                        .WithMessage(CardRules.MoveCardRequiresPositionErrorMessage);
                })
                .When(x => x.TargetListId.IsAssigned);
        }
    }
}
