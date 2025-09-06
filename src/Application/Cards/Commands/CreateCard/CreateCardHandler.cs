using Application.Cards.DTOs;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Domain.Entities;
using MediatR;

namespace Application.Cards.Commands.CreateCard
{
    public class CreateCardHandler : IRequestHandler<CreateCardCommand, CardDetailsDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IAppAuthorizationService _authService;
        public CreateCardHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser, 
            IAppAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }
        public async Task<CardDetailsDto> Handle(CreateCardCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            if (!(await _authService.CanAccessListAsync(EntityOperations.AddToParent, request.ListId, userId, cancellationToken)))
                throw new NotFoundException("You are not authorized or project/board is not found.");

            var card = new Card
            {
                Title = request.Title,
                CardListId = request.ListId,
                Position = request.Position
            };

            await _context.Cards.AddAsync(card);
            await _context.SaveChangesAsync(cancellationToken);
            return new CardDetailsDto(
                card.Id,
                card.CardListId,
                card.Title,
                card.Description,
                card.DueAt,
                card.Position
            );
        }
    }
}
