using Application.Boards.Commands.CreateBoard;
using Application.Boards.Commands.DeleteBoard;
using Application.Boards.Commands.UpdateBoard;
using Application.Boards.DTOs;
using Application.Boards.Queries.GetBoardById;
using Application.Cards.Queries.GetBoardWorksoace;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[ApiVersion("1")]
[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class BoardController : ControllerBase
{
    private readonly IMediator _mediator;
    public BoardController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateBoardRequest request)
    {
        var id = await _mediator.Send(new CreateBoardCommand(request.ProjectId, request.Title, request.Description));
        return CreatedAtAction(nameof(GetById), new { request.ProjectId, boardId = id, version = HttpContext.GetRequestedApiVersion()?.ToString() }, new { id });
    }

    [HttpGet("{boardId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid boardId, CancellationToken ct)
    {
        var cmd = new GetBoardByIdQuery(boardId);
        var board = await _mediator.Send(cmd, ct);
        return Ok(board);
    }

    [HttpPut("{boardId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromRoute] Guid boardId, [FromBody] UpdateBoardRequest request, CancellationToken ct)
    {
        var cmd = new UpdateBoardCommand(boardId, request.Title, request.Description);
        await _mediator.Send(cmd, ct);

        return NoContent();
    }

    [HttpDelete("{boardId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid boardId, CancellationToken ct)
    {
        var cmd = new DeleteBoardCommand(boardId);
        await _mediator.Send(cmd, ct);
        return NoContent();
    }

    [HttpGet("{boardId:guid}/workspace")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBoardCards([FromRoute] Guid boardId)
    {
        var cards = await _mediator.Send(new GetBoardWorkspaceQuery(boardId));
        return Ok(cards);
    }

}
