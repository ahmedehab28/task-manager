using Application.Boards.DTOs;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Boards.Queries.GetAllBoards
{
    public class GetAllBoardsHandler : IRequestHandler<GetAllBoardsQuery, IEnumerable<BoardDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IProjectAuthorizationService _authService;

        public GetAllBoardsHandler(
            IApplicationDbContext cotnext,
            ICurrentUser currentUser,
            IProjectAuthorizationService authService)
        {
            _context = cotnext;
            _currentUser = currentUser;
            _authService = authService;
        }
        public async Task<IEnumerable<BoardDto>> Handle(GetAllBoardsQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            if (!(await _authService.IsProjectMemberAsync(request.ProjectId, userId, cancellationToken)))
                throw new NotFoundException("You are not authorized or board is not found.");

            var boards = await _context.Boards
                .AsNoTracking()
                .Where(b => b.ProjectId == request.ProjectId)
                .OrderBy(b => b.CreatedAt)  
                .Select(b => new BoardDto (b.Id, b.Title, b.Description, b.CreatedAt))
                .ToListAsync(cancellationToken);

            return boards;
        }
    }
}
