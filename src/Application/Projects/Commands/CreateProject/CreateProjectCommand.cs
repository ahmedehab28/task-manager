using Application.Projects.DTOs;
using MediatR;

namespace Application.Projects.Commands.CreateProject
{
    public record CreateProjectCommand(
        string Title,
        string? Description) : IRequest<ProjectDto>;
}
