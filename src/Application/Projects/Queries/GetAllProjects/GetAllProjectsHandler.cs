using Application.Common.Interfaces;
using Application.Projects.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Queries.GetAllProjects
{
    public class GetAllProjectsHandler : IRequestHandler<GetAllProjectsQuery, IEnumerable<ProjectDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        public GetAllProjectsHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<IEnumerable<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;

            var projects = await (
                from pm in _context.ProjectMembers
                join p in _context.Projects on pm.ProjectId equals p.Id
                where pm.UserId == userId
                select new ProjectDto(
                    p.Id,
                    p.Title,
                    p.Description
                )
            ).ToListAsync(cancellationToken);

            return projects;
        }
    }
}
