
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.Common.Enums;
using MediatR;

namespace Application.Projects.Commands.CreateProject
{
    internal class CreateProjectHandler : IRequestHandler<CreateProjectCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;

        public CreateProjectHandler(IApplicationDbContext applicationDbContext,
                                    ICurrentUser currentUser)
        {
            _context = applicationDbContext;
            _currentUser = currentUser;
        }
        public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            var newProject = new Project
            {
                Title = request.Title,
                Description = request.Description,
            };

            var projectMember = new ProjectMember
            {
                ProjectId = newProject.Id,
                UserId = userId,
                Role = ProjectRole.Owner
            };

            await _context.Projects.AddAsync(newProject, cancellationToken);
            await _context.ProjectMembers.AddAsync(projectMember, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return newProject.Id;
        }
    }
}
