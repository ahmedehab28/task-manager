using Application.ProjectMembers.Commands.AddProjectMember;
using Application.ProjectMembers.Commands.DeleteMember;
using Application.ProjectMembers.Commands.UpdateMember;
using Application.ProjectMembers.DTOs;
using Application.Projects.DTOs;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/project/{projectId:guid}/member")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]

    public class ProjectMemberController : Controller
    {
        private readonly IMediator _mediator;
        public ProjectMemberController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddProjectMember([FromRoute] Guid projectId, [FromBody] AddProjectMemberRequest request, CancellationToken ct)
        {
            var cmd = new AddProjectMemberCommand(projectId, request.UserId, request.Role);
            await _mediator.Send(cmd, ct);
            return Created();
        }

        [HttpDelete("{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveMember([FromRoute] Guid projectId, [FromRoute] Guid userId, CancellationToken ct)
        {
            var cmd = new DeleteProjectMemberCommand(projectId, userId);
            await _mediator.Send(cmd, ct);
            return NoContent();
        }

        [HttpPut("{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateMember([FromRoute] Guid projectId, [FromRoute] Guid userId, [FromBody] UpdateProjectMemberRequest request, CancellationToken ct)
        {
            var cmd = new UpdateMemberCommand(projectId, userId, request.Role);
            await _mediator.Send(cmd, ct);
            return NoContent();
        }




    }
}
