using Domain.Enums;

namespace Application.ProjectMembers.DTOs
{
    public record ProjectMembersListDto(
        Guid ProjectId,
        string ProejctTitle,
        IList<ProjectMemberDto> Members);
}
