using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Domain.Entities;
using MediatR;

namespace Application.ProjectMembers.Commands.AddProjectMember
{
    public class AddProjectMemberHandler : IRequestHandler<AddProjectMemberCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IProjectAuthorizationService _authService;

        public AddProjectMemberHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser, 
            IProjectAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }

        public async Task Handle(AddProjectMemberCommand request, CancellationToken cancellationToken)
        {
            // We can implement this using raw sql to hit the db once
            var currentUserId = _currentUser.Id;
            if (!await _authService.IsProjectAdminAsync(request.ProjectId, currentUserId, cancellationToken))
                throw new NotFoundException("You are not authorized to add member to this project or it's not found.");

            var newMember = new ProjectMember
            {
                ProjectId = request.ProjectId,
                UserId = request.UserId,
                Role = request.Role,
            };

            await _context.ProjectMembers.AddAsync(newMember, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
