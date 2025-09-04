using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Boards.Commands.DeleteBoard
{
    public class DeleteBoardHandler : IRequestHandler<DeleteBoardCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IBoardAuthorizationService _authService;

        public DeleteBoardHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser,
            IBoardAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }

        public async Task Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            if (!(await _authService.CanAccessBoardAsync(request.ProjectId, request.BoardId, userId, cancellationToken)))
                throw new NotFoundException("You are not authorized or board is not found.");

            var board = await _context.Boards.FindAsync(request.BoardId, cancellationToken);

            _context.Boards.Remove(board!);
            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
