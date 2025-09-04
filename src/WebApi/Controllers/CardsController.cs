using Application.Cards.Commands.CreateCard;
using Application.Cards.Commands.DeleteCard;
using Application.Cards.Commands.MoveCard;
using Application.Cards.Commands.UpdateCard;
using Application.Cards.DTOs;
using Application.Cards.Queries.GetCardById;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/projects/{projectId:guid}/boards/{boardId:guid}/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class CardsController : Controller
    {
        private readonly IMediator _mediator;
        public CardsController(
            IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [ProducesResponseType(typeof(CardDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateCard([FromRoute] Guid projectId, [FromRoute] Guid boardId, [FromBody] CreateCardRequest request)
        {
            var result = await _mediator.Send(new CreateCardCommand(projectId, boardId, request.ListId, request.Title));
            return CreatedAtAction(nameof(GetCardById), new { projectId, boardId, id = result.Id }, result);
        }


        [HttpGet("{cardId:guid}")]
        [ProducesResponseType(typeof(CardDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCardById([FromRoute] Guid projectId, [FromRoute] Guid boardId, [FromRoute] Guid cardId)
        {
            var card = await _mediator.Send(new GetCardByIdQuery(projectId, boardId, cardId));
            return Ok(card);
        }

        [HttpPut("{cardId:guid}")]
        [ProducesResponseType(typeof(CardDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCardDetails([FromRoute] Guid projectId, [FromRoute] Guid boardId, [FromRoute] Guid cardId, UpdateCardDetailsRequest request)
        {
            var result = await _mediator.Send(new UpdateCardDetailsCommand(projectId, boardId, cardId, request.Title, request.Description, request.DueAt));
            return Ok(result);
        }

        [HttpPatch("{cardId:guid}/move")]
        [ProducesResponseType(typeof(CardDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MoveCard([FromRoute] Guid projectId, [FromRoute] Guid boardId, [FromRoute] Guid cardId, MoveCardRequest request)
        {
            var card = await _mediator.Send(new MoveCardCommand(projectId, boardId, request.PrevCardId, request.NextCardId, request.TargetListId, cardId));
            return Ok(card);
        }

        [HttpDelete("{cardId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCard([FromRoute] Guid projectId, [FromRoute] Guid boardId, [FromRoute] Guid cardId)
        {
            await _mediator.Send(new DeleteCardCommand(projectId, boardId, cardId));
            return NoContent();

        }
    }
}
