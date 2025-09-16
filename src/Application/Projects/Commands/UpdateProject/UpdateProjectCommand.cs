using MediatR;

namespace Application.Projects.Commands.UpdateProject
{
    public record UpdateProjectCommand(
        Guid ProjectId,
        string? Title,
        string? Description) : IRequest
    {
    }
}
