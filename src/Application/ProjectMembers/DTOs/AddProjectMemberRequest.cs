using Domain.Enums;

namespace Application.ProjectMembers.DTOs
{
    public record AddProjectMemberRequest(
        Guid UserId,
        ProjectRole Role)
    {
    }
}
