namespace Application.Cards.DTOs
{
    public record MoveCardRequest(
        Guid? PrevCardId,
        Guid? NextCardId,
        Guid? TargetListId);
}
