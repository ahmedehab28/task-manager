using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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

        public async Task<bool> CanMoveCardAsync(Guid listId, Guid cardId, Guid userId, CancellationToken cancellation)
        {
            var canAccessCard = _context.Cards
                    .AnyAsync(c =>
                        c.Id == cardId &&
                        c.CardList.Board.Project.Members.Any(pm => pm.UserId == userId));

            var canAccessList = _appAuthService.CanAccessListAsync(EntityOperations.Update, listId, userId, cancellation);

            await Task.WhenAll(canAccessCard, canAccessList);
            return canAccessCard.Result && canAccessList.Result;
        }
      

    }
}
