
using FluentValidation;

namespace Application.Boards.Queries.GetAllBoards
{
    public class GetAllBoardsValidator : AbstractValidator<GetAllBoardsQuery>
    {
        public GetAllBoardsValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("ProjectId is required.");
        }
    }
}
