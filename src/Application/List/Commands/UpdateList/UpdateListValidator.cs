using FluentValidation;

namespace Application.List.Commands.UpdateList
{
    public class UpdateListValidator : AbstractValidator<UpdateListCommand>
    {
        public UpdateListValidator()
        {
            RuleFor(x => x.ListId)
            .NotEmpty().WithMessage("ListId is required.");

            RuleFor(x => x.Title)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Title cannot be empty.")
                .MaximumLength(50).WithMessage("Title cannot exceed 50 characters.")
                .When(x => x.Title != null);

            RuleFor(x => x.BoardId)
                .Must(id => id != Guid.Empty)
                .WithMessage("BoardId must be a valid GUID.")
                .When(x => x.BoardId.HasValue);

            RuleFor(x => x.Position)
                .NotNull()
                .When(x => x.BoardId.HasValue)
                .WithMessage("Position is required when moving a list to another board.");

            RuleFor(cmd => cmd.Position)
                .GreaterThanOrEqualTo(0m).WithMessage("Position must be zero or positive.")
                .PrecisionScale(18, 4, false).WithMessage("Position can have up to 4 decimal places and no more than 18 digits total.")
                .When(x => x.Position.HasValue);
        }
    }
}
