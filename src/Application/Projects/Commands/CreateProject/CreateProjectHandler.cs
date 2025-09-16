using Application.Common.Interfaces;
using Application.Projects.DTOs;
using Domain.Entities;
using MediatR;

namespace Application.Projects.Commands.CreateProject
{
    public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;

        public CreateProjectHandler(IApplicationDbContext applicationDbContext,
                                    ICurrentUser currentUser)
        {
            _context = applicationDbContext;
            _currentUser = currentUser;
        }
        public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            var newProject = Project.CreateWithDefaultInbox(request.Title, request.Description, userId);

            _context.Projects.Add(newProject);
            await _context.SaveChangesAsync(cancellationToken);

            return new ProjectDto(
                newProject.Id,
                newProject.Title,
                newProject.Description);
        }
    }
}
