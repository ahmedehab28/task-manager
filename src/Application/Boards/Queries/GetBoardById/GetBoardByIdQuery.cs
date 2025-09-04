
using Application.Boards.DTOs;
using MediatR;

namespace Application.Boards.Queries.GetBoardById
{
    public record GetBoardByIdQuery(
        Guid ProjectId,
        Guid BoardId) : IRequest<BoardDto>;
}
