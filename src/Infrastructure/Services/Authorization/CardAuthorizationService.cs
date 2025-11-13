using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Authorization
{
    public class CardAuthorizationService : ICardAuthorizationService
    {
        private readonly IApplicationDbContext _context;
        private readonly IAppAuthorizationService _appAuthService;

        public CardAuthorizationService(
            IApplicationDbContext context,
            IAppAuthorizationService appAuthService)
        {
            _context = context;
            _appAuthService = appAuthService;
        }

        public async Task<bool> CanAssignMembersToCardAsync(Guid cardId, Guid userId, CancellationToken cancellation)
        {
            return await _context.ProjectMembers
                .AnyAsync(pm => 
                    pm.UserId == userId &&
                    (pm.Role == ProjectRole.Admin || pm.Role == ProjectRole.Owner) &&
                    pm.Project.Boards.Any(b => 
                        b.CardLists.Any(cl =>
                            cl.Cards.Any(c => c.Id == cardId))),
                    cancellation);
        }
      

    }
}
