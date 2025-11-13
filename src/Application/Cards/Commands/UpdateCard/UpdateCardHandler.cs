using Application.Cards.DTOs;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using MediatR;

namespace Application.Cards.Commands.UpdateCard
{
    public class UpdateCardHandler : IRequestHandler<UpdateCardCommand, CardDetailsDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IAppAuthorizationService _authService;
        public UpdateCardHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser,
            IAppAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }
        public async Task<CardDetailsDto> Handle(UpdateCardCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            var card = await _authService.GetCardAsync(EntityOperations.Update, request.CardId, userId, cancellationToken)
                ?? throw new NotFoundException("You are not authorized or card is not found.");

            if (request.Title.IsAssigned)
                card.Title = request.Title.Value!;
            if (request.Description.IsAssigned)
                card.Description = request.Description.Value;
            if (request.DueAt.IsAssigned)
                card.DueAt = request.DueAt.Value;
            if (request.Position.IsAssigned)
                card.Position = request.Position.Value!.Value;
            if (request.TargetListId.IsAssigned)
            {
                if (!(await _authService.CanAccessListAsync(EntityOperations.AddToParent, request.TargetListId.Value!.Value, userId, cancellationToken)))
                    throw new NotFoundException("You are not authorized or target list is not found.");
                card.CardListId = request.TargetListId.Value!.Value;
            }
            await _context.SaveChangesAsync(cancellationToken);
            return new CardDetailsDto(
                card.Id,
                card.CardListId,
                card.Title,
                card.Description,
                card.DueAt,
                card.Position);
        }
    }
}