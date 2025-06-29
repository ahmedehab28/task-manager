using Application.Interfaces;
using Domain.Entities.Boards;
using MediatR;

namespace Application.Boards.Commands.CreateBoard
{
    public class CreateBoardHandler : IRequestHandler<CreateBoardCommand, Guid>
    {
        private readonly IBoardRepository _repository;
        public CreateBoardHandler(IBoardRepository repository)
        {
            _repository = repository;
        }
        public async Task<Guid> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
        {
            var board = new Board(request.Title, request.Description);

            await _repository.AddAsync(board);
            await Task.CompletedTask;   // Placeholder for later async methods

            return board.Id;
        }
    }
}
