using Application.ProjectMembers.DTOs;
using MediatR;

namespace Application.ProjectMembers.Queries.GetProjectMembers
{
    public record GetProjectMembersQuery(
        Guid ProjectId) : IRequest<ProjectMembersListDto>
    {
    }
}
