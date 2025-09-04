using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Application.List.DTOs;
using Application.List.Services;
using Domain.Entities;
using MediatR;

namespace Application.List.Commands.CreateList
{
    public class CreateListHandler : IRequestHandler<CreateListCommand, ListDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IBoardAuthorizationService _authService;
        private readonly IListPositionService _listPosService;
        public CreateListHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser,
            IBoardAuthorizationService authService,
            IListPositionService listPosService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
            _listPosService = listPosService;
        }
        public async Task<ListDto> Handle(CreateListCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;

            if (!(await _authService.CanAccessBoardAsync(request.ProjectId, request.BoardId, userId, cancellationToken)))
                throw new KeyNotFoundException("You are not authorized or project/board is not found.");

            var newPosition = await _listPosService.GetNewListPositionAsync(request.BoardId, cancellationToken);

            var cardList = new CardList
            {
                BoardId = request.BoardId,
                Title = request.Title,
                Position = newPosition
            };

            await _context.CardLists.AddAsync(cardList, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new ListDto
            (
                cardList.BoardId,
                cardList.Id,
                cardList.Title,
                cardList.Position
            );
        }
    }
}
