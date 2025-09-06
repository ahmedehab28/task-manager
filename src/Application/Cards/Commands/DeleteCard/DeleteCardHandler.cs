using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using MediatR;

namespace Application.Cards.Commands.DeleteCard
{
    public class DeleteCardHandler : IRequestHandler<DeleteCardCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IAppAuthorizationService _authService;

        public DeleteCardHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser,
            IAppAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }

        public async Task Handle(DeleteCardCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            if (!(await _authService.CanAccessCardAsync(EntityOperations.Delete, request.CardId, userId, cancellationToken)))
                throw new KeyNotFoundException("You are not authorized or project/board/card is not found.");

            var card = await _context.Cards.FindAsync(new object?[] { request.CardId }, cancellationToken);
            
            _context.Cards.Remove(card!);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
