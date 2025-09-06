using Application.Cards.DTOs;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using MediatR;

namespace Application.Cards.Commands.MoveCard
{
    public class MoveCardHandler : IRequestHandler<MoveCardCommand, CardDetailsDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly ICardAuthorizationService _authService;

        public MoveCardHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser,
            ICardAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }

        public async Task<CardDetailsDto> Handle(MoveCardCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;

            if (!(await _authService.CanMoveCardAsync(request.ListId, request.Id, userId, cancellationToken)))
                throw new NotFoundException("One or more cards could not be found or you are not authorized to access them.");

            var card = (await _context.Cards.FindAsync(new object[] { request.Id }, cancellationToken))!;

            card.CardListId = request.ListId;
            card.Position = request.Position;

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
