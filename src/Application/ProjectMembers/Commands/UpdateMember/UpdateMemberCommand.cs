using Domain.Enums;
using MediatR;

namespace Application.ProjectMembers.Commands.UpdateMember
{
    public record UpdateMemberCommand(
        Guid ProjectId,
        Guid UserId,
        ProjectRole Role) : IRequest;
}
