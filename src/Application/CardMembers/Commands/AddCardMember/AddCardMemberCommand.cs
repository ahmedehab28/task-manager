using MediatR;

namespace Application.CardMembers.Commands.AddCardMember
{
    public record AddCardMemberCommand(
        Guid CardId,
        Guid UserId
        ) : IRequest;
}
