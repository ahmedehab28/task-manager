using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Boards.Commands.CreateBoard;
using Asp.Versioning;
using Application.Boards.DTOs;
using Application.Boards.Queries.GetAllBoards;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BoardsController : ControllerBase
{
    private readonly IMediator _mediator;
    public BoardsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBoardDto request)
    {
        var cmd = new CreateBoardCommand(request.Title, request.Description);
        var id = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(Create), new { id }, new { id });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cmd = new GetAllBoardsQuery();
        var boards = await _mediator.Send(cmd);
        return Ok(boards);
    }

}
