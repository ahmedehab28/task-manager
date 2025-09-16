using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using MediatR;

namespace Application.Projects.Commands.UpdateProject
{
    public class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IProjectAuthorizationService _authService;
        public UpdateProjectHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser,
            IProjectAuthorizationService authService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
        }
        public async Task Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            if (!(await _authService.IsProjectAdminAsync(request.ProjectId, userId, cancellationToken)))
                throw new NotFoundException("You are either not authorized to update this project or it's not found.");

            var project = await _context.Projects.FindAsync(
                [request.ProjectId],
                cancellationToken: cancellationToken)
                ?? throw new NotFoundException("Project is not found.");

            if (request.Title is not null)
                project.Title = request.Title;
            if (request.Description is not null)
                project.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
