using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Projects.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Queries.GetProjectById
{
    public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        public GetProjectByIdHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<ProjectDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            var project = await ( 
                from pm in _context.ProjectMembers
                join p in _context.Projects on pm.ProjectId equals p.Id
                where pm.UserId == userId && pm.ProjectId == request.ProjectId
                select new ProjectDto(p.Id, p.Title, p.Description)
            ).FirstOrDefaultAsync(cancellationToken);

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
