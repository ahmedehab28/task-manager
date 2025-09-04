using Application.Cards.DTOs;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using MediatR;

namespace Application.Cards.Queries.GetCardById
{
    public class GetCardByIdHandler : IRequestHandler<GetCardByIdQuery, CardDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly ICardAuthorizationService _authService;
        public GetCardByIdHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser,
            ICardAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }
        public async Task<CardDto> Handle(GetCardByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            if (!(await _authService.CanAccessCardsAsync(request.ProjectId, request.BoardId, null, new List<Guid> { request.CardId }, userId, cancellationToken)).ContainsKey(request.CardId))
                throw new UnauthorizedAccessException("You are not authorized or resourse is not found.");

            var card = await _context.Cards.FindAsync(new object?[] { request.CardId }, cancellationToken);
            if (card == null || card.BoardId != request.BoardId || card.Board.ProjectId != request.ProjectId)
                throw new KeyNotFoundException("Card not found.");

            return new CardDto(
                card.Id,
                card.BoardId,
                card.ListId,
                card.Title,
                card.Description,
                card.Position,
                card.DueAt
            );
        }
    }
}
