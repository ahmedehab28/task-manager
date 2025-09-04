using Application.Cards.DTOs;
using Application.Cards.Services;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using MediatR;

namespace Application.Cards.Commands.MoveCard
{
    public class MoveCardHandler : IRequestHandler<MoveCardCommand, CardDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly ICardAuthorizationService _authService;
        private readonly ICardPositionService _CardPosService;

        public MoveCardHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser,
            ICardAuthorizationService authService,
            ICardPositionService cardPosService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
            _CardPosService = cardPosService;
        }

        public async Task<CardDto> Handle(MoveCardCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            var relevantCardIds = new List<Guid> { request.CardId };
            if (request.PrevCardId.HasValue) relevantCardIds.Add(request.PrevCardId.Value);
            if (request.NextCardId.HasValue) relevantCardIds.Add(request.NextCardId.Value);

            using var transaction = await _context.BeginTransactionAsync(cancellationToken);
            try
            {
                var authorizedCards = await _authService.CanAccessCardsAsync(
                        request.ProjectId, request.BoardId, request.TargetListId, relevantCardIds, userId, cancellationToken);

                bool allCardsAuthorized = true;
                if (!authorizedCards.ContainsKey(request.CardId)) allCardsAuthorized = false;
                if (request.PrevCardId.HasValue && !authorizedCards.ContainsKey(request.PrevCardId.Value)) allCardsAuthorized = false;
                if (request.NextCardId.HasValue && !authorizedCards.ContainsKey(request.NextCardId.Value)) allCardsAuthorized = false;

                if (!allCardsAuthorized)
                    throw new NotFoundException("One or more cards could not be found or you are not authorized to access them.");

                var newCardPosition = await _CardPosService.GetMovedCardPositionAsync(
                    request.BoardId,
                    request.TargetListId,
                    request.PrevCardId,
                    request.NextCardId,
                    request.CardId,
                    cancellationToken);

                var card = (await _context.Cards.FindAsync(new object[] { request.CardId }, cancellationToken))!;

                card.ListId = request.TargetListId;
                card.Position = newCardPosition;

                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return new CardDto(
                    card.Id,
                    card.BoardId,
                    card.ListId,
                    card.Title,
                    card.Description,
                    card.Position,
                    card.DueAt);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
