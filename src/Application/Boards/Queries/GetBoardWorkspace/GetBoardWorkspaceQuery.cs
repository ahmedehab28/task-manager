using Application.Boards.DTOs;
using Domain.Enums;
using MediatR;

namespace Application.Cards.Queries.GetBoardWorksoace
{
    public record GetBoardWorkspaceQuery(
        Guid BoardId) : IRequest<BoardDto>;
}
