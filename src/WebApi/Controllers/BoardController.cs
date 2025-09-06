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
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
[Produces("application/json")]
public class BoardController : ControllerBase
{
    private readonly IMediator _mediator;
    public BoardController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(BoardDetailsDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateBoardRequest request)
    {
        var newBoard = await _mediator.Send(new CreateBoardCommand(request.ProjectId, request.Title, request.Description));
        return CreatedAtAction(nameof(GetById), new { boardId = newBoard.Id,  }, new { newBoard });
    }

    [HttpGet("{boardId:guid}")]
    [ProducesResponseType(typeof(BoardDetailsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromRoute] Guid boardId, CancellationToken ct)
    {
        var cmd = new GetBoardByIdQuery(boardId);
        var board = await _mediator.Send(cmd, ct);
        return Ok(board);
    }

    [HttpPut("{boardId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update([FromRoute] Guid boardId, [FromBody] UpdateBoardRequest request, CancellationToken ct)
    {
        var cmd = new UpdateBoardCommand(boardId, request.Title, request.Description);
        await _mediator.Send(cmd, ct);

        return NoContent();
    }

    [HttpDelete("{boardId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] Guid boardId, CancellationToken ct)
    {
        var cmd = new DeleteBoardCommand(boardId);
        await _mediator.Send(cmd, ct);
        return NoContent();
    }

    [HttpGet("{boardId:guid}/workspace")]
    [ProducesResponseType(typeof(BoardDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBoardCards([FromRoute] Guid boardId)
    {
        var cards = await _mediator.Send(new GetBoardWorkspaceQuery(boardId));
        return Ok(cards);
    }

}
