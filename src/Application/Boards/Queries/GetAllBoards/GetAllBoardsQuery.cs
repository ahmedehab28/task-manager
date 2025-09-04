using Application.Boards.DTOs;
using MediatR;

namespace Application.Boards.Queries.GetAllBoards
{
    public record GetAllBoardsQuery(
        Guid ProjectId) : IRequest<IEnumerable<BoardDto>>;
}
