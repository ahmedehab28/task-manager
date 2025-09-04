using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Projects.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Queries.GetProjectById
{
    public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentUser _currentUser;
        public GetProjectByIdHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUser currentUser)
        {
            _applicationDbContext = applicationDbContext;
            _currentUser = currentUser;
        }
        public async Task<ProjectDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            var project = await _applicationDbContext.ProjectMembers
                .Where(pm => pm.UserId == userId && pm.ProjectId == request.ProjectId)
                .Select(pm => pm.Project)
                .FirstOrDefaultAsync(cancellationToken);

            if (project == null)
                throw new NotFoundException($"Project with ID {request.ProjectId} not found or access denied.");

            var dto = new ProjectDto(
                Id: project.Id,
                Title: project!.Title,
                Description: project.Description);

            return dto;
        }
    }
}
