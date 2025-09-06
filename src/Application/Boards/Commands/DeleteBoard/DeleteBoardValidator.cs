
using FluentValidation;

namespace Application.Boards.Commands.DeleteBoard
{
    public class DeleteBoardValidator : AbstractValidator<DeleteBoardCommand>
    {
        public DeleteBoardValidator()
        {
            RuleFor(x => x.BoardId)
                .NotEmpty().WithMessage("Board Id is required.");
        }
    }
}
