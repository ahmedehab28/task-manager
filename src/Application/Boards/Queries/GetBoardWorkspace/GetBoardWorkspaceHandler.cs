using Application.Boards.DTOs;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Cards.Queries.GetBoardWorksoace
{
    public class GetBoardWorkspaceHandler : IRequestHandler<GetBoardWorkspaceQuery, BoardDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IAppAuthorizationService _authService;
        public GetBoardWorkspaceHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser,
            IAppAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }
        public async Task<BoardDto> Handle(GetBoardWorkspaceQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            if (!(await _authService.CanAccessBoardAsync(EntityOperations.View, request.BoardId, userId, cancellationToken)))
                throw new NotFoundException("You are not authorized or resourse is not found.");

            var cardLists = await _context.CardLists
                .AsNoTracking()
                .Where(cl => cl.BoardId == request.BoardId)
                .OrderBy(cl => cl.Position)
                .Select(cl => new BoardListsDto(
                    cl.Id,
                    cl.BoardId,
                    cl.Title,
                    cl.Position,
                    cl.Cards
                        .OrderBy(c => c.Position)
                        .Select(c => new BoardCardsDto(
                            c.Id,
                            cl.BoardId,
                            c.CardListId,
                            c.Title,
                            c.Description,
                            c.DueAt,
                            c.Position))
                        .ToList()))
                .ToListAsync(cancellationToken);

            return new BoardDto(
                request.BoardId,
                cardLists);
        }
    }
}
