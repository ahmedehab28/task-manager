using MediatR;

namespace Application.List.Commands.DeleteList
{
    public record DeleteListCommand(
        Guid ProjectId,
        Guid BoardId,
        Guid ListId) : IRequest;
    {
    }
}
