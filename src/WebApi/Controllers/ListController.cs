using Application.List.Commands.CreateList;
using Application.List.Commands.DeleteList;
using Application.List.Commands.UpdateList;
using Application.List.DTOs;
using Application.List.Queries.GetListById;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("/api/{version:apiVersion}/[controller]")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public class ListController : Controller
    {
        private readonly IMediator _mediator;
        public ListController(IMediator mediatior)
        {
            _mediator = mediatior;
        }
        [HttpPost]
        [ProducesResponseType(typeof(ListDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateListRequest request)
        {
            var list = await _mediator.Send(new CreateListCommand(request.BoardId, request.Title, request.Position));

            return CreatedAtAction(nameof(GetListById), new { listId = list.Id}, list);
        }

        [HttpGet("{listId:guid}")]
        [ProducesResponseType(typeof(ListDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListById([FromRoute] Guid listId)
        {
            var list = await _mediator.Send(new GetListByIdQuery(listId));
            return Ok(list);
        }

        [HttpPut("{listId:guid}")]
        [ProducesResponseType(typeof(ListDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateList([FromRoute] Guid listId, [FromBody] UpdateListRequest request)
        {
            var list = await _mediator.Send(new UpdateListCommand(listId, request.BoardId, request.Title, request.Position));
            return Ok(list);
        }

        [HttpDelete("{listId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteList([FromRoute] Guid listId)
        {
            await _mediator.Send(new DeleteListCommand(listId));
            return NoContent();
        }


    }
}
