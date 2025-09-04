using MediatR;

namespace Application.Boards.Commands.UpdateBoard
{
    public record UpdateBoardCommand(
        Guid ProjectId,
        Guid BoardId,
        string Title, 
        string? Description) : IRequest;
}
