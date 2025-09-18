using Application.Boards.DTOs;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Boards.Queries.GetAllBoards
{
    public class GetAllBoardsHandler : IRequestHandler<GetAllBoardsQuery, IEnumerable<BoardDetailsDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IAppAuthorizationService _authService;

        public GetAllBoardsHandler(
            IApplicationDbContext cotnext,
            ICurrentUser currentUser,
            IAppAuthorizationService authService)
        {
            _context = cotnext;
            _currentUser = currentUser;
            _authService = authService;
        }
        public async Task<IEnumerable<BoardDetailsDto>> Handle(GetAllBoardsQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            if (!(await _authService.CanAccessProject(EntityOperations.View, request.ProjectId, userId, cancellationToken)))
                throw new NotFoundException("You are not authorized or board is not found.");

            var boards = await _context.Boards
                .AsNoTracking()
                .Where(b => b.ProjectId == request.ProjectId)
                .OrderBy(b => b.CreatedAt)
                .ThenBy(b => b.Id)
                .Select(b => new BoardDetailsDto (b.Id, b.ProjectId, b.Title, b.Description))
                .ToListAsync(cancellationToken);

            return boards;
        }
    }
}
