using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Boards.Commands.CreateBoard
{
    public class CreateBoardHandler : IRequestHandler<CreateBoardCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IAppAuthorizationService _authService;
        public CreateBoardHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUser currentUser,
            IAppAuthorizationService authService)
        {
            _context = applicationDbContext;
            _currentUser = currentUser;
            _authService = authService;
        }
        public async Task<Guid> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            if (!(await _authService.CanAccessProject(EntityOperations.AddToParent, request.ProjectId, userId, cancellationToken)))
                throw new NotFoundException("You are not authorized or project is not found.");

            var board = new Board
            {
                Title = request.Title,
                Description = request.Description,
                ProjectId = request.ProjectId,
                BoardType = BoardType.Normal,
            };

            await _context.Boards.AddAsync(board, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return board.Id;
        }
    }
}
