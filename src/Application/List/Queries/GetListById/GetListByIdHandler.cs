using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Application.List.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.List.Queries.GetListById
{
    public class GetListByIdHandler : IRequestHandler<GetListByIdQuery, ListDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IAppAuthorizationService _authService;
        public GetListByIdHandler (
            IApplicationDbContext context,
            ICurrentUser currentUser,
            IAppAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }
        public async Task<ListDto> Handle(GetListByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;

            var list = await _authService.GetListAsync(EntityOperations.View, request.ListId, userId, cancellationToken)
                ?? throw new NotFoundException("You are not authorized or card is not found.");

            return new ListDto(
                list.Id,
                list.BoardId,
                list.Title,
                list.Position);
        }
    }
}
