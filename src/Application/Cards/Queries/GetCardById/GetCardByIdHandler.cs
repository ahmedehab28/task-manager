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
        private readonly IUserService _userService;
        public GetCardByIdHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser,
            IAppAuthorizationService authService,
            IUserService userService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
            _userService = userService;
        }
        public async Task<CardDto> Handle(GetCardByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;

            if (!(await _authService.CanAccessCardAsync(EntityOperations.View, request.CardId, userId, cancellationToken)))
                throw new NotFoundException("You are not authorized or card is not found.");

            var card = await _context.Cards
                .Include(c => c.Members)
                .SingleOrDefaultAsync(c => c.Id == request.CardId, cancellationToken);

            var memberIds = card!.Members.Select(cm => cm.UserId).ToList();

            var cardMembers = await _userService.GetByIdsAsync(memberIds, cancellationToken);

            var memberDtos = cardMembers.Values.Select(cm => new CardMemberDto(cm.Id, cm.UserName)).ToList();

            return new CardDto(
                    card.Id,
                    card.CardListId,
                    card.Title,
                    card.Description,
                    card.DueAt,
                    card.Position,
                    memberDtos);
        }
    }
}
