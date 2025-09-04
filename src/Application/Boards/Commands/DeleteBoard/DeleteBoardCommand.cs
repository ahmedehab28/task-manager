using MediatR;

namespace Application.Boards.Commands.DeleteBoard
{
    public record DeleteBoardCommand(
        Guid ProjectId,
        Guid BoardId) : IRequest
    {
    }
}
