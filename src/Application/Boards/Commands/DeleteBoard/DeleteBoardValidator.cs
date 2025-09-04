
using FluentValidation;

namespace Application.Boards.Commands.DeleteBoard
{
    public class DeleteBoardValidator : AbstractValidator<DeleteBoardCommand>
    {
        public DeleteBoardValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("Project Id is required.");
            RuleFor(x => x.BoardId)
                .NotEmpty().WithMessage("Board Id is required.");
        }
    }
}
