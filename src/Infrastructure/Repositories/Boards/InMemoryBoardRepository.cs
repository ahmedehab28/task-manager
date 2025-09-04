using Application.Common.Interfaces;
using Domain.Entities;

namespace Infrastructure.Repositories.Boards
{
    public class InMemoryBoardRepository : IBoardRepository
    {
        private readonly List<Board> _boards = [];
        public Task AddAsync(Board board)
        {
            ArgumentNullException.ThrowIfNull(board);
            _boards.Add(board);
            return Task.CompletedTask;
        }

        public Task<Board?> GetByIdAsync(Guid id)
        {
            var board = _boards.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(board ?? null);

        }
        public Task<IEnumerable<Board>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Board>>(_boards);
        }
        public async Task UpdateAsync(Board newBoard)
        {
            Board? existing = await GetByIdAsync(newBoard.Id);
            if (existing != null)
            {
                existing.Title = newBoard.Title;
                existing.Description = newBoard.Description;
            }
        }
        public async Task DeleteAsync(Guid id)
        {
            Board? board = await GetByIdAsync(id);
            if (board != null)
                _boards.Remove(board);
        }
    }
}
