using FluentValidation;

namespace Application.Boards.Queries.GetBoardById
{
    public class GetBoardByIdValidator : AbstractValidator<GetBoardByIdQuery>
    {
        public GetBoardByIdValidator()
        {
            RuleFor(x => x.BoardId)
                .NotEmpty().WithMessage("Board Id is required.");
        }
    }
}
