namespace Application.Cards.DTOs
{
    public record MoveCardRequest(
        Guid ListId,
        decimal Position);
}
