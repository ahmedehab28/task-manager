
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Domain.Entities.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Authorization
{
    public class ProjectAuthorizationService : IProjectAuthorizationService
    {
        private readonly IApplicationDbContext _context;
        public ProjectAuthorizationService(
            IApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<bool> IsProjectOwnerAsync(Guid projectId, Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.ProjectMembers
                .AnyAsync(
                    pm => pm.ProjectId == projectId &&
                    pm.UserId == userId && 
                    pm.Role == ProjectRole.Owner, cancellationToken);
        }
        public async Task<bool> IsProjectAdminAsync(Guid projectId, Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.ProjectMembers
                .AnyAsync(
                    pm => pm.ProjectId == projectId && 
                    pm.UserId == userId && 
                    (pm.Role == ProjectRole.Admin || pm.Role == ProjectRole.Owner), cancellationToken);
        }

        public async Task<bool> IsProjectMemberAsync(Guid projectId, Guid userId, CancellationToken cancellation)
        {
            return await _context.ProjectMembers
                .AnyAsync(
                    pm => pm.ProjectId == projectId && 
                    pm.UserId == userId, cancellation);
        }

    }
}
