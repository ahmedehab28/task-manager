using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Domain.Enums;
using MediatR;

namespace Application.ProjectMembers.Commands.DeleteMember
{
    public class DeleteProjectMemberHandler : IRequestHandler<DeleteProjectMemberCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IProjectAuthorizationService _authService;

        public DeleteProjectMemberHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser,
            IProjectAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }
        public async Task Handle(DeleteProjectMemberCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUser.Id;

            if (!await _authService.IsProjectAdminAsync(request.ProjectId, currentUserId, cancellationToken))
                throw new NotFoundException("You are not authorized to delete member to this project or it's not found.");

            var projectMember = await _context.ProjectMembers
                .FindAsync(new object[] { request.ProjectId, request.DeleteUserId }, cancellationToken) ?? 
                throw new NotFoundException("User doesn't exist in this project.");

            if (projectMember.Role == ProjectRole.Owner)
                throw new ForbiddenAccessException("You are not authorized to delete the owner.");

            _context.ProjectMembers.Remove(projectMember!);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
