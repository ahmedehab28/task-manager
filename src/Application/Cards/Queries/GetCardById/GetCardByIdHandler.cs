using Application.Cards.DTOs;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Cards.Queries.GetCardById
{
    public class GetCardByIdHandler : IRequestHandler<GetCardByIdQuery, CardDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IAppAuthorizationService _authService;
        public GetCardByIdHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser,
            IAppAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }
        public async Task<CardDto> Handle(GetCardByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            var card = await _authService.GetCardAsync(EntityOperations.View, request.CardId, userId, cancellationToken)
                ?? throw new NotFoundException("You are not authorized or card is not found.");

            var cardMembers = await (from cm in _context.CardMembers
                                     join u in _context.Users on cm.UserId equals u.Id
                                     where cm.CardId == request.CardId
                                     select new CardMemberDto(cm.UserId, u.UserName))
                                     .ToListAsync(cancellationToken);

            return new CardDto(
                    card.Id,
                    card.CardListId,
                    card.Title,
                    card.Description,
                    card.DueAt,
                    card.Position,
                    cardMembers);
        }
    }
}
