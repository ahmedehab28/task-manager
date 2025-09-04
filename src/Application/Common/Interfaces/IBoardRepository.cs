using Domain.Entities;

namespace Application.Common.Interfaces
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
