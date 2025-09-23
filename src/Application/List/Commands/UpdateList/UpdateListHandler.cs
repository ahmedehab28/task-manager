using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Application.List.DTOs;
using MediatR;

namespace Application.List.Commands.UpdateList
{
    public class UpdateListHandler : IRequestHandler<UpdateListCommand, ListDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IAppAuthorizationService _authService;

        public UpdateListHandler(
            IApplicationDbContext context, 
            ICurrentUser currentUser, 
            IAppAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }

        public async Task<ListDto> Handle(UpdateListCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;

            var list = await _authService.GetListAsync(EntityOperations.Update, request.ListId, userId, cancellationToken)
                ?? throw new NotFoundException("You are not authorized or List is not found.");

            if (request.Title != null)
                list.Title = request.Title;
            if (request.Position != null)
            {
                list.Position = request.Position.Value;
                if (request.BoardId.HasValue)
                {
                    if(!(await _authService.CanAccessBoardAsync(EntityOperations.Update, request.BoardId.Value, userId, cancellationToken)))
                        throw new NotFoundException("You are not authorized or Target Board is not found.");
                    list.BoardId = request.BoardId.Value;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
            return new ListDto(
                request.ListId,
                list.BoardId,
                list.Title,
                list.Position);
        }
    }
}
