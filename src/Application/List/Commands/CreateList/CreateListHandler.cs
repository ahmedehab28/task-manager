using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Application.List.DTOs;
using Domain.Entities;
using MediatR;

namespace Application.List.Commands.CreateList
{
    public class CreateListHandler : IRequestHandler<CreateListCommand, ListDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IAppAuthorizationService _authService;
        public CreateListHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser,
            IAppAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }
        public async Task<ListDto> Handle(CreateListCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;

            if (!(await _authService.CanAccessBoardAsync(EntityOperations.AddToParent, request.BoardId, userId, cancellationToken)))
                throw new NotFoundException("You are not authorized or board is not found.");

            var cardList = new CardList
            {
                BoardId = request.BoardId,
                Title = request.Title,
                Position = request.Position
            };

            await _context.CardLists.AddAsync(cardList, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new ListDto
            (
                cardList.Id,
                cardList.BoardId,
                cardList.Title,
                cardList.Position
            );
        }
    }
}
