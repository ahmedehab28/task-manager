using Domain.Enums;

namespace Application.ProjectMembers.DTOs
{
    public record ProjectMemberDto(
        Guid UserId,
        string Username,
        string DisplayName,
        ProjectRole Role,
        DateTime JoinedAt);
}
