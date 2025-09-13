using FluentValidation;

namespace Application.ProjectMembers.Commands.UpdateMember
{
    public class UpdateMemberValidator : AbstractValidator<UpdateMemberCommand>
    {
        public UpdateMemberValidator() 
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("ProjectId is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("Role must be a valid ProjectRole value.")
                .NotEmpty().WithMessage("Role must be specified.");
        }
    }
}
