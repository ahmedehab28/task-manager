using Application.Cards.DTOs;
using MediatR;

namespace Application.Cards.Commands.MoveCard
{
    public record MoveCardCommand(
        Guid ProjectId,
        Guid BoardId,
        Guid? PrevCardId,
        Guid? NextCardId,
        Guid? TargetListId,
        Guid CardId) : IRequest<CardDto>
    {
    }
}
