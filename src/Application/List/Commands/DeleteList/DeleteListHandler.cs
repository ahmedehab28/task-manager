using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using MediatR;

namespace Application.List.Commands.DeleteList
{
    public class DeleteListHandler : IRequestHandler<DeleteListCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IBoardAuthorizationService _authService;
        public DeleteListHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser,
            IBoardAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }
        public async Task Handle(DeleteListCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            if (!await (_authService.CanAccessBoardAsync(request.ProjectId, request.BoardId, userId, cancellationToken)))
                throw new NotFoundException("You are not authorized or project/board is not found.");
        }
    }
}
