using FluentValidation;

namespace Application.ProjectMembers.Commands.AddProjectMember
{
    public class AddProjectMemberValidator : AbstractValidator<AddProjectMemberCommand>
    {
        public AddProjectMemberValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("Role must be a valid ProjectRole value.")
                .NotEmpty().WithMessage("Role must be specified.");
        }
    }
}
