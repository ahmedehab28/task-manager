using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Domain.Enums;
using MediatR;

namespace Application.ProjectMembers.Commands.UpdateMember
{
    public class UpdateMemberHandler : IRequestHandler<UpdateMemberCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IProjectAuthorizationService _authService;

        public UpdateMemberHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser, 
            IProjectAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }

        public async Task Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUser.Id;

            if (!await _authService.IsProjectAdminAsync(request.ProjectId, currentUserId, cancellationToken))
                throw new NotFoundException("You are not authorized to update members in this project or it's not found.");

            var projectMember = await _context.ProjectMembers
                .FindAsync(new object[] { request.ProjectId, request.UserId }, cancellationToken) ??
                throw new NotFoundException("User doesn't exist in this project.");

            if (projectMember.Role == ProjectRole.Owner)
                throw new ForbiddenAccessException("You are not authorized to update the owner.");

            projectMember.Role = request.Role;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
