using Application.Cards.DTOs;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using MediatR;

namespace Application.Cards.Commands.UpdateCard
{
    public class UpdateCardDetailsHandler : IRequestHandler<UpdateCardDetailsCommand, CardDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly ICardAuthorizationService _authService;
        public UpdateCardDetailsHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser,
            ICardAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }
        public async Task<CardDto> Handle(UpdateCardDetailsCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            if (!(await _authService.CanAccessCardAsync(request.ProjectId, request.BoardId, null, request.CardId, userId, cancellationToken)))
                throw new NotFoundException("You are not authorized or project/board/card is not found.");
            var card = (await _context.Cards.FindAsync(new object?[] { request.CardId }, cancellationToken))!;

            if (request.Title != null && card.Title != request.Title)
                card.Title = request.Title;
            if (request.Description != null && card.Description != request.Description)
                card.Description = request.Description;
            if (request.DueAt != null && card.DueAt != request.DueAt)
                card.DueAt = request.DueAt;
            await _context.SaveChangesAsync(cancellationToken);
            return new CardDto(
                card.Id,
                card.BoardId,
                card.ListId,
                card.Title,
                card.Description,
                card.Position,
                card.DueAt);
        }
    }
}
