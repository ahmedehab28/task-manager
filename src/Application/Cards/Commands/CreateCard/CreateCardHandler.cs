using Application.Cards.DTOs;
using Application.Cards.Services;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Domain.Entities;
using MediatR;

namespace Application.Cards.Commands.CreateCard
{
    public class CreateCardHandler : IRequestHandler<CreateCardCommand, CardDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly ICardAuthorizationService _authService;
        private readonly ICardPositionService _cardPosService;
        public CreateCardHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser, 
            ICardAuthorizationService authService,
            ICardPositionService cardPosService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
            _cardPosService = cardPosService;
        }
        public async Task<CardDto> Handle(CreateCardCommand request, CancellationToken cancellationToken)
        {

            using (var transaction = await _context.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var userId = _currentUser.Id;
                    if (!(await _authService.CanCreateCardAsync(request.ProjectId, request.BoardId, request.ListId, userId, cancellationToken)))
                        throw new NotFoundException("You are not authorized or project/board is not found.");

                    var card = new Card
                    {
                        Title = request.Title,
                        BoardId = request.BoardId,
                        ListId = request.ListId
                    };

                    var newCardPosition = await _cardPosService.GetNewCardPositionAsync(request.BoardId, request.ListId, cancellationToken);
                    card.Position = newCardPosition;

                    await _context.Cards.AddAsync(card);
                    await _context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return new CardDto(
                        card.Id,
                        card.BoardId,
                        card.ListId,
                        card.Title,
                        card.Description,
                        card.Position,
                        card.DueAt
                    );
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }
    }
}
