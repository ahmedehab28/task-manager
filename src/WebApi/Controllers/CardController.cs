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
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public class CardController : Controller
    {
        private readonly IMediator _mediator;
        public CardController(
            IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [ProducesResponseType(typeof(CardDetailsDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCard([FromBody] CreateCardRequest request)
        {
            var result = await _mediator.Send(new CreateCardCommand(request.ListId, request.Title, request.Position));
            return CreatedAtAction(nameof(GetCardById), new { cardId = result.Id }, result);
        }


        [HttpGet("{cardId:guid}")]
        [ProducesResponseType(typeof(CardDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCardById([FromRoute] Guid cardId)
        {
            var card = await _mediator.Send(new GetCardByIdQuery(cardId));
            return Ok(card);
        }

        [HttpPut("{cardId:guid}")]
        [ProducesResponseType(typeof(CardDetailsDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCardDetails([FromRoute] Guid cardId, UpdateCardDetailsRequest request)
        {
            var result = await _mediator.Send(new UpdateCardDetailsCommand(cardId, request.Title, request.Description, request.DueAt));
            return Ok(result);
        }

        [HttpPatch("{cardId:guid}/move")]
        [ProducesResponseType(typeof(CardDetailsDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> MoveCard([FromRoute] Guid cardId, MoveCardRequest request)
        {
            var card = await _mediator.Send(new MoveCardCommand(cardId, request.ListId, request.Position));
            return Ok(card);
        }

        [HttpDelete("{cardId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteCard([FromRoute] Guid projectId, [FromRoute] Guid boardId, [FromRoute] Guid cardId)
        {
            await _mediator.Send(new DeleteCardCommand(projectId, boardId, cardId));
            return NoContent();

        }
    }
}
