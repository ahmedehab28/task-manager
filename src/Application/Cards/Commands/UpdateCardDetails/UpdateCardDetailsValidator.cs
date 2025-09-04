using FluentValidation;

namespace Application.Cards.Commands.UpdateCard
{
    public class UpdateCardDetailsValidator : AbstractValidator<UpdateCardDetailsCommand>
    {
        public UpdateCardDetailsValidator() 
        {
            RuleFor(x => x.ProjectId).NotEmpty();
            RuleFor(x => x.BoardId).NotEmpty();
            RuleFor(x => x.CardId).NotEmpty();
            RuleFor(x => x.Title).MaximumLength(100).When(x => x.Title != null);
            RuleFor(x => x.Description).MaximumLength(1000).When(x => x.Description != null);
            RuleFor(x => x.DueAt).GreaterThan(DateTime.UtcNow).When(x => x.DueAt != null);
        }
    }
}
