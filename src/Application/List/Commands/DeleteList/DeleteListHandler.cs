using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using MediatR;

namespace Application.List.Commands.DeleteList
{
    public class DeleteListHandler : IRequestHandler<DeleteListCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IAppAuthorizationService _authService;
        public DeleteListHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser,
            IAppAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }
        public async Task Handle(DeleteListCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            var list = await _authService.GetListAsync(EntityOperations.Delete, request.ListId, userId, cancellationToken)
                ?? throw new NotFoundException("You are not authorized or list is not found.");

            _context.CardLists.Remove(list);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
