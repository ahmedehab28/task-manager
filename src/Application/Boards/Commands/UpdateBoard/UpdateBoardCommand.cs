using MediatR;

namespace Application.Boards.Commands.UpdateBoard
{
    public record UpdateBoardCommand(
        Guid BoardId,
        string? Title, 
        string? Description) : IRequest;
}
