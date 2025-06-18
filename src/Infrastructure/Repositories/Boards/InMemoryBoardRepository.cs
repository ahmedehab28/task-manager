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
        public void Add(Board board)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Board> GetAll()
        {
            throw new NotImplementedException();
        }

        public Board? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(Board board)
        {
            throw new NotImplementedException();
        }
    }
}
