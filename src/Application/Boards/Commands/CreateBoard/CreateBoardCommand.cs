using MediatR;


namespace Application.Boards.Commands.CreateBoard
{
    public sealed record CreateBoardCommand (string Title, string? Description) : IRequest<Guid>;
}
