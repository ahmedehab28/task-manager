using Domain.Rules;
using FluentValidation;


namespace Application.Boards.Commands.CreateBoard
{
    public class CreateBoardValidator : AbstractValidator<CreateBoardCommand>
    {
        public CreateBoardValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .WithMessage("ProjectId is required.");

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
