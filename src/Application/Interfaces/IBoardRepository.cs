using Domain.Entities.Boards;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBoardRepository
    {
        public void Add(Board board);
        public Board? GetById(Guid id);
        public IEnumerable<Board> GetAll();
        public void Update (Board board);
        public void Delete(Guid id);
    }
}
