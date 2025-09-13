using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Application.ProjectMembers.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Application.ProjectMembers.Queries.GetProjectMembers
{
    public class GetProjectMembersHandler : IRequestHandler<GetProjectMembersQuery, ProjectMembersListDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IAppAuthorizationService _authService;
        private readonly IUserService _userService;

        public GetProjectMembersHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser,
            IAppAuthorizationService authService,
            IUserService userService)
        {
            _context = context;
            _currentUser = currentUser;
            _authService = authService;
            _userService = userService;
        }

        public async Task<ProjectMembersListDto> Handle(GetProjectMembersQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUser.Id;

            var projectMembers = await _context.Projects
                .Where(p =>
                    p.Id == request.ProjectId &&
                    p.Members.Any(pm => pm.UserId == currentUserId))
                .Select(p => new ProjectMembersListDto(
                    p.Id,
                    p.Title,
                    p.Members.Select(pm => new ProjectMemberDto(
                        pm.UserId,
                        pm.User.UserName,
                        pm.User.FirstName + " " + pm.User.LastName,
                        pm.Role,
                        pm.JoinedAt
                    )).ToList()
                ))
                .FirstOrDefaultAsync(cancellationToken);

            return projectMembers == null ?
                throw new NotFoundException("You are not authorized to access this project or no members found")
                :
                projectMembers;
                
                

                        

        }
    }
}
