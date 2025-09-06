
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
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
            var newProject = Project.CreateWithDefaultInbox(request.Title, request.Description, userId);

            _context.Projects.Add(newProject);
            await _context.SaveChangesAsync(cancellationToken);

            return newProject.Id;
        }
    }
}
