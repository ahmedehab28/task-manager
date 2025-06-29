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
        Task AddAsync(Board board);
        Task<Board?> GetByIdAsync(Guid id);
        Task<IEnumerable<Board>> GetAllAsync();
        Task UpdateAsync(Board board);
        Task DeleteAsync(Guid id);
    }
}
