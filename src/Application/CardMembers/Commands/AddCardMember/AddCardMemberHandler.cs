using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Domain.Entities;
using MediatR;

namespace Application.CardMembers.Commands.AddCardMember
{
    public class AddCardMemberHandler : IRequestHandler<AddCardMemberCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly ICardAuthorizationService _authService;
        public AddCardMemberHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser,
            ICardAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }

        public async Task Handle(AddCardMemberCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            if (!(await _authService.CanAssignMembersToCardAsync(request.CardId, userId, cancellationToken)))
                throw new NotFoundException("You are not authorized or card is not found.");

            var newMember = new CardMember
            {
                CardId = request.CardId,
                UserId = request.UserId
            };
            await _context.CardMembers.AddAsync(newMember);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
