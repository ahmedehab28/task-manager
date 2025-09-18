using FluentValidation;

namespace Application.Cards.Queries.GetBoardWorksoace
{
    public class GetBoardWorkspaceValidator : AbstractValidator<GetBoardWorkspaceQuery>
    {
        public GetBoardWorkspaceValidator()
        {
            RuleFor(x => x.BoardId)
                .NotEmpty().WithMessage("BoardId is required.");
        }
    }
}
