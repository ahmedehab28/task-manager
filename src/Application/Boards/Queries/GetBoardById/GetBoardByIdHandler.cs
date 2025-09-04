using Application.Boards.DTOs;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Boards.Queries.GetBoardById
{
    public class GetBoardByIdHandler : IRequestHandler<GetBoardByIdQuery, BoardDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IBoardAuthorizationService _authService;
        public GetBoardByIdHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser,
            IBoardAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }
        public async Task<BoardDto> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            if (!(await _authService.CanAccessBoardAsync(request.ProjectId, request.BoardId, userId, cancellationToken)))
                throw new NotFoundException("You are not authorized or board is not found.");

            var board = await _context.Boards
                .Where(b => b.Id == request.BoardId && b.ProjectId == request.ProjectId)
                .Select(b => new BoardDto(b.Id, b.Title, b.Description, b.CreatedAt))
                .FirstOrDefaultAsync(cancellationToken);

            if (board is null)
                throw new NotFoundException($"Board with ID {request.BoardId} not found in this project.");

            return board;
        }
    }
}
