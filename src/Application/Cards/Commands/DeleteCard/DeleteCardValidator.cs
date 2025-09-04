using FluentValidation;

namespace Application.Cards.Commands.DeleteCard
{
    public class DeleteCardValidator : AbstractValidator<DeleteCardCommand>
    {
        public DeleteCardValidator() 
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("ProjectId is required.");
            RuleFor(x => x.BoardId)
                .NotEmpty().WithMessage("BoardId is required.");
            RuleFor(x => x.CardId)
                .NotEmpty().WithMessage("CardId is required.");
        }
    }
}
