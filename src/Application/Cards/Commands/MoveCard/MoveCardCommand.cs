using Application.Cards.DTOs;
using MediatR;

namespace Application.Cards.Commands.MoveCard
{
    public record MoveCardCommand(
        Guid Id,
        Guid ListId,
        decimal Position) : IRequest<CardDetailsDto>
    {
    }
}
