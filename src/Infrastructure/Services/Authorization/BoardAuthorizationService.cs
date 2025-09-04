using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Authorization
{
    public class BoardAuthorizationService : IBoardAuthorizationService
    {
        private readonly IApplicationDbContext _context;
        public BoardAuthorizationService(
            IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CanAccessBoardAsync(Guid projectId, Guid boardId, Guid userId,  CancellationToken cancellationToken)
        {
            return await _context.Boards
                .AnyAsync(
                    b => b.Id == boardId &&
                    b.ProjectId == projectId &&
                    b.Project.Members.Any(pm => pm.UserId == userId),
                    cancellationToken);
        }
    }
}
