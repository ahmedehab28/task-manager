using FluentValidation;

namespace Application.CardMembers.Commands.AddCardMember
{
    public class AddCardMemberValidator : AbstractValidator<AddCardMemberCommand>
    {
        public AddCardMemberValidator()
        {
            RuleFor(x => x.CardId)
                .NotEmpty().WithMessage("CardId is required.");
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");
        }
    }
}
