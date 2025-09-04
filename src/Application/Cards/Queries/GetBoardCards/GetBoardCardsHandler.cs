using Application.Cards.DTOs;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using MediatR;

namespace Application.Cards.Queries.GetAllCards
{
    public class GetBoardCardsHandler : IRequestHandler<GetBoardCardsQuery, IEnumerable<CardDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IBoardAuthorizationService _authService;
        public GetBoardCardsHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser,
            IBoardAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }
        public async Task<IEnumerable<CardDto>> Handle(GetBoardCardsQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            if (!(await _authService.CanAccessBoardAsync(request.ProjectId, request.BoardId, userId, cancellationToken)))
                throw new NotFoundException("You are not authorized or resourse is not found.");

            var cards = _context.Cards
                .Where(c =>
                    c.BoardId == request.BoardId &&
                    c.Board.ProjectId == request.ProjectId)
                .Select(c => new CardDto(
                    c.Id,
                    c.BoardId,
                    c.ListId,
                    c.Title,
                    c.Description,
                    c.Position,
                    c.DueAt
                ));
            return cards;


        }
    }
}
