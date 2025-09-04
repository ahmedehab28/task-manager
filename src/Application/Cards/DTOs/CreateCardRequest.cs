namespace Application.Cards.DTOs
{
    public record CreateCardRequest(
        Guid? ListId,
        string Title)
    {
    }
}
