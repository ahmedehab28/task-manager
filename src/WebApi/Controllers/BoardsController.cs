using Application.Boards.Commands.CreateBoard;
using Application.Boards.Commands.DeleteBoard;
using Application.Boards.Commands.UpdateBoard;
using Application.Boards.DTOs;
using Application.Boards.Queries.GetAllBoards;
using Application.Boards.Queries.GetBoardById;
using Application.Cards.Queries.GetAllCards;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[ApiVersion("1")]
[Authorize]
[Route("api/v{version:apiVersion}/projects/{projectId:guid}/[controller]")]
public class BoardsController : ControllerBase
{
    private readonly IMediator _mediator;
    public BoardsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromRoute] Guid projectId, [FromBody] CreateBoardDto request)
    {
        var id = await _mediator.Send(new CreateBoardCommand(projectId, request.Title, request.Description));
        return CreatedAtAction(nameof(GetById), new { projectId, boardId = id, version = HttpContext.GetRequestedApiVersion()?.ToString() }, new { id });
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAll([FromRoute] Guid projectId, CancellationToken ct)
    {
        var cmd = new GetAllBoardsQuery(projectId);
        var boards = await _mediator.Send(cmd, ct);
        return Ok(boards);
    }

    [HttpGet("{boardId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid projectId, [FromRoute] Guid boardId, CancellationToken ct)
    {
        var cmd = new GetBoardByIdQuery(projectId, boardId);
        var board = await _mediator.Send(cmd, ct);
        return Ok(board);
    }

    [HttpPut("{boardId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromRoute] Guid projectId, [FromRoute] Guid boardId, [FromBody] UpdateBoardDto board, CancellationToken ct)
    {
        var cmd = new UpdateBoardCommand(projectId, boardId, board.title, board.description);
        await _mediator.Send(cmd, ct);

        return NoContent();
    }

    [HttpDelete("{boardId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid projectId, [FromRoute] Guid boardId, CancellationToken ct)
    {
        var cmd = new DeleteBoardCommand(projectId, boardId);
        await _mediator.Send(cmd, ct);
        return NoContent();
    }

    [HttpGet("{boardId:guid}/cards")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBoardCards([FromRoute] Guid projectId, [FromRoute] Guid boardId)
    {
        var cards = await _mediator.Send(new GetBoardCardsQuery(projectId, boardId));
        return Ok(cards);
    }

}
