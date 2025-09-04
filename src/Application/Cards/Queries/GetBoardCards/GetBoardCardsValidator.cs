using FluentValidation;

namespace Application.Cards.Queries.GetAllCards
{
    public class GetBoardCardsValidator : AbstractValidator<GetBoardCardsQuery>
    {
        public GetBoardCardsValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("ProjectId is required.");
            RuleFor(x => x.BoardId)
                .NotEmpty().WithMessage("BoardId is required.");
        }
    }
}
