using FluentValidation;

namespace Application.List.Commands.DeleteList
{
    public class DeleteListValidator : AbstractValidator<DeleteListCommand>
    {
        public DeleteListValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("ProjectId is required.");
            RuleFor(x => x.BoardId)
                .NotEmpty().WithMessage("BoardId is required.");
            RuleFor(x => x.ListId)
                .NotEmpty().WithMessage("ListId is required.");
        }
    }
}
