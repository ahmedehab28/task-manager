using MediatR;

namespace Application.Cards.Commands.DeleteCard
{
    public record DeleteCardCommand(
        Guid CardId) : IRequest
    {
    }
}
