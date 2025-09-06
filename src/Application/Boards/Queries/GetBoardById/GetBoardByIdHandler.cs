using Application.Boards.DTOs;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Boards.Queries.GetBoardById
{
    public class GetBoardByIdHandler : IRequestHandler<GetBoardByIdQuery, BoardDetailsDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IAppAuthorizationService _authService;
        public GetBoardByIdHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser,
            IAppAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }
        public async Task<BoardDetailsDto> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            if (!(await _authService.CanAccessBoardAsync(EntityOperations.View, request.BoardId, userId, cancellationToken)))
                throw new NotFoundException("You are not authorized or board is not found.");

            var board = await _context.Boards
                .Where(b => b.Id == request.BoardId)
                .Select(b => new BoardDetailsDto(
                    b.Id,
                    b.ProjectId,
                    b.Title,
                    b.Description))
                .FirstOrDefaultAsync(cancellationToken);

            return board!;
        }
    }
}
