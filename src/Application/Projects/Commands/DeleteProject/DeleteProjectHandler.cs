
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using MediatR;

namespace Application.Projects.Commands.DeleteProject
{
    public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IProjectAuthorizationService _authService;
        public DeleteProjectHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser,
            IProjectAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }
        public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            if (!(await _authService.IsProjectOwnerAsync(request.ProjectId, userId, cancellationToken)))
                throw new NotFoundException("You are either not authorized to delete this project or it's not found.");

            var project = await _context.Projects.FindAsync(request.ProjectId, cancellationToken) 
                ?? throw new NotFoundException("Project is not found.");

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
