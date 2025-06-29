using Application.Boards.DTOs;
using Application.Interfaces;
using MediatR;


namespace Application.Boards.Queries.GetAllBoards
{
    public class GetAllBoardsHandler : IRequestHandler<GetAllBoardsQuery, List<BoardDto>>
    {
        private readonly IBoardRepository _boardRepository;
        public GetAllBoardsHandler(IBoardRepository boardRepository)
        {
            _boardRepository = boardRepository;
        }
        public async Task<List<BoardDto>> Handle(GetAllBoardsQuery request, CancellationToken cancellationToken)
        {
            var boards = await _boardRepository.GetAllAsync();
            var boardsDto = boards.Select(b => new BoardDto(
                    Id: b.Id,
                    Title: b.Title,
                    Description: b.Description!,
                    CreatedAt: b.CreatedAt)).ToList();
            return boardsDto;
        }
    }
}
