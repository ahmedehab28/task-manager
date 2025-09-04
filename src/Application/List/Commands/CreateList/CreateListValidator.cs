using FluentValidation;

namespace Application.List.Commands.CreateList
{
    public class CreateListValidator : AbstractValidator<CreateListCommand>
    {
        public CreateListValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("ProjectId is required.");
            RuleFor(x => x.BoardId)
                .NotEmpty().WithMessage("BoardId is required.");
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");
        }
    }
}
