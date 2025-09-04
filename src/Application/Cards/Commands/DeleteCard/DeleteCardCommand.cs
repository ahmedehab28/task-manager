using MediatR;

namespace Application.Cards.Commands.DeleteCard
{
    public record DeleteCardCommand(
        Guid ProjectId,
        Guid BoardId,
        Guid CardId) : IRequest
    {
    }
}
