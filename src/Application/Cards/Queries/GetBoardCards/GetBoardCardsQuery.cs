using Application.Cards.DTOs;
using MediatR;

namespace Application.Cards.Queries.GetAllCards
{
    public record GetBoardCardsQuery(
        Guid ProjectId,
        Guid BoardId) : IRequest<IEnumerable<CardDto>>
    {
    }
}
