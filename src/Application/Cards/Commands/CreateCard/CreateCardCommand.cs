using Application.Cards.DTOs;
using MediatR;

namespace Application.Cards.Commands.CreateCard
{
    public record CreateCardCommand(
        Guid ListId,
        string Title,
        decimal Position) : IRequest<CardDetailsDto>
    {
    }
}
