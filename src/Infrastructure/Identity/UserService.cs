using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class UserService : IUserService
    {
        private readonly IApplicationDbContext _context;

        public UserService(
            IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserDto(u.Id, u.UserName!, u.Email!, u.FirstName))
                .SingleOrDefaultAsync(ct);

            return user;
        }

        public async Task<IReadOnlyDictionary<Guid, UserDto>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var set = ids.ToHashSet();
            var users = await _context.Users
                .Where(u => set.Contains(u.Id))
                .Select(u => new UserDto(u.Id, u.UserName!, u.Email!, u.FirstName))
                .ToListAsync(ct);

            return users.ToDictionary(u => u.Id);
        }
    }
}
