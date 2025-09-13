using Application.Boards.Queries.GetAllBoards;
using Application.ProjectMembers.Queries.GetProjectMembers;
using Application.Projects.Commands.CreateProject;
using Application.Projects.Commands.DeleteProject;
using Application.Projects.Commands.UpdateProject;
using Application.Projects.DTOs;
using Application.Projects.Queries.GetAllProjects;
using Application.Projects.Queries.GetProjectById;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Authorize]
    [Route("api/v{version:apiversion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]

    public class ProjectController : Controller
    {
        private readonly IMediator _mediator;
        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateProjectRequest request, CancellationToken ct)
        {
            var cmd = new CreateProjectCommand(request.Title, request.Description);
            var result = await _mediator.Send(cmd, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, new { id = result });
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var query = new GetProjectByIdQuery(id);
            var result = await _mediator.Send(query, ct);
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProjectDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var query = new GetAllProjectsQuery();
            var result = await _mediator.Send(query, ct);
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProjectRequest request, CancellationToken ct)
        {
            var cmd = new UpdateProjectCommand(id, request.Title, request.Description);
            await _mediator.Send(cmd, ct);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        {
            var cmd = new DeleteProjectCommand(id);
            await _mediator.Send(cmd, ct);
            return NoContent();
        }

        [HttpGet("{projectId:guid}/boards")]
        [ProducesResponseType(typeof(IEnumerable<ProjectDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllProjectBoards([FromRoute] Guid projectId, CancellationToken ct)
        {
            var cmd = new GetAllBoardsQuery(projectId);
            var boards = await _mediator.Send(cmd, ct);
            return Ok(boards);
        }

        [HttpGet("{projectId:guid}/members")]
        [ProducesResponseType(typeof(IEnumerable<ProjectDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllProjectMembers([FromRoute] Guid projectId, CancellationToken ct)
        {
            var cmd = new GetProjectMembersQuery(projectId);
            var members = await _mediator.Send(cmd, ct);
            return Ok(members);
        }
    }
}
