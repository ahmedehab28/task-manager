using MediatR;

namespace Application.ProjectMembers.Commands.DeleteMember
{
    public record DeleteProjectMemberCommand(
        Guid ProjectId,
        Guid DeleteUserId) : IRequest;
}
