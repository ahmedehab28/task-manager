using Domain.Rules;
using FluentValidation;

namespace Application.Boards.Commands.UpdateBoard
{
    public class UpdateBoardValidator : AbstractValidator<UpdateBoardCommand>
    {
        public UpdateBoardValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(BoardRules.TitleMaxLength)
                .WithMessage($"Title cannot exceed {BoardRules.TitleMaxLength} characters.");

            RuleFor(x => x.Description)
                .MaximumLength(BoardRules.DescriptionMaxLength)
                .WithMessage($"Description cannot exceed {BoardRules.DescriptionMaxLength} characters.");
        }
    }
}
