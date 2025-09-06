using FluentValidation;

namespace Application.Cards.Queries.GetBoardWorksoace
{
    public class GetBoardWorksoaceValidator : AbstractValidator<GetBoardWorkspaceQuery>
    {
        public GetBoardWorksoaceValidator()
        {
            RuleFor(x => x.BoardId)
                .NotEmpty().WithMessage("BoardId is required.");
        }
    }
}
