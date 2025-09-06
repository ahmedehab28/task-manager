using Application.Common.Models;

namespace Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyDictionary<Guid, UserDto>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}
