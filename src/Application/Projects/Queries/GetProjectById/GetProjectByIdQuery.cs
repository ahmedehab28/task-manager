using Application.Projects.DTOs;
using MediatR;

namespace Application.Projects.Queries.GetProjectById
{
    public record GetProjectByIdQuery(
        Guid ProjectId) : IRequest<ProjectDto>;
}
