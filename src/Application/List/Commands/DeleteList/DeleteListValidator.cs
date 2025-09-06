using FluentValidation;

namespace Application.List.Commands.DeleteList
{
    public class DeleteListValidator : AbstractValidator<DeleteListCommand>
    {
        public DeleteListValidator()
        {
            RuleFor(x => x.ListId)
                .NotEmpty().WithMessage("ListId is required.");
        }
    }
}
