using Application.Cards.DTOs;
using MediatR;

namespace Application.Cards.Queries.GetCardById
{
    public record GetCardByIdQuery(
        Guid ProjectId,
        Guid BoardId,
        Guid CardId) : IRequest<CardDto>;
}
