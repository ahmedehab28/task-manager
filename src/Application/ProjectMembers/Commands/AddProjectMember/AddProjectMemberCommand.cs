using Domain.Enums;
using MediatR;

namespace Application.ProjectMembers.Commands.AddProjectMember
{
    public record AddProjectMemberCommand(
        Guid ProjectId,
        Guid UserId,
        ProjectRole Role) : IRequest;
}