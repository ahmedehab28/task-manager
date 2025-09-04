using Application.Projects.DTOs;
using MediatR;

namespace Application.Projects.Queries.GetAllProjects
{
    public record GetAllProjectsQuery() : IRequest<IEnumerable<ProjectDto>>;
}
