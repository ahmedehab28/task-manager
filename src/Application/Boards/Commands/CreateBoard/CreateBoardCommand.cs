using Application.Boards.DTOs;
using MediatR;


namespace Application.Boards.Commands.CreateBoard
{
    public sealed record CreateBoardCommand (
        Guid ProjectId,
        string Title, 
        string? Description) : IRequest<BoardDetailsDto>;
}
