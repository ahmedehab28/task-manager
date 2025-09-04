using Application.Cards.DTOs;
using MediatR;

namespace Application.Cards.Commands.UpdateCard
{
    public record UpdateCardDetailsCommand(
        Guid ProjectId,
        Guid BoardId,
        Guid CardId,
        string? Title,
        string? Description,
        DateTime? DueAt) : IRequest<CardDto>
    {
    }
}
