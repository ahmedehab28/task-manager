using Application.Cards.DTOs;
using MediatR;

namespace Application.Cards.Commands.UpdateCard
{
    public record UpdateCardDetailsCommand(
        Guid CardId,
        string? Title,
        string? Description,
        DateTime? DueAt) : IRequest<CardDetailsDto>
    {
    }
}
