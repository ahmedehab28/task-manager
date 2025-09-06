using MediatR;

namespace Application.List.Commands.DeleteList
{
    public record DeleteListCommand(
        Guid ListId) : IRequest;
}
