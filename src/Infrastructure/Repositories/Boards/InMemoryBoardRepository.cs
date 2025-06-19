using Application.Interfaces;
using Domain.Entities.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Boards
{
    public class InMemoryBoardRepository : IBoardRepository
    {
        private readonly List<Board> _boards = [];
        public void Add(Board board)
        {
            ArgumentNullException.ThrowIfNull(board);
            _boards.Add(board);
        }

        public Board? GetById(Guid id)
        {
            var board = _boards.FirstOrDefault(x => x.Id == id);
            return board ?? null;

        }
        public IEnumerable<Board> GetAll()
        {
            return _boards;
        }
        public void Update(Board newBoard)
        {
            Board? existing = GetById(newBoard.Id);
            if (existing != null)
            {
                existing.UpdateTitle(newBoard.Title);
                existing.UpdateDescription(newBoard.Description!);
            }
        }
        public void Delete(Guid id)
        {
            Board? board = GetById(id);
            if (board != null)
                _boards.Remove(board);
        }
    }
}
