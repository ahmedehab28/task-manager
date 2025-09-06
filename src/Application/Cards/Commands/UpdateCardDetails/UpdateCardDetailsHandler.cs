using Application.Cards.DTOs;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Cards.Commands.UpdateCard
{
    public class UpdateCardDetailsHandler : IRequestHandler<UpdateCardDetailsCommand, CardDetailsDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IAppAuthorizationService _authService;
        public UpdateCardDetailsHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser,
            IAppAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }
        public async Task<CardDetailsDto> Handle(UpdateCardDetailsCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            if (!(await _authService.CanAccessCardAsync(EntityOperations.Update, request.CardId, userId, cancellationToken)))
                throw new NotFoundException("You are not authorized or card is not found.");

            var card = (await _context.Cards
                .FirstAsync(c => c.Id == request.CardId , cancellationToken))!;

            if (request.Title != null && card.Title != request.Title)
                card.Title = request.Title;
            if (request.Description != null && card.Description != request.Description)
                card.Description = request.Description;
            if (request.DueAt != null && card.DueAt != request.DueAt)
                card.DueAt = request.DueAt;
            await _context.SaveChangesAsync(cancellationToken);
            return new CardDetailsDto(
                card.Id,
                card.CardListId,
                card.Title,
                card.Description,
                card.DueAt,
                card.Position);
        }
    }
}
