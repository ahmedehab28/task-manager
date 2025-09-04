namespace Application.Cards.DTOs
{
    public record UpdateCardDetailsRequest(
        string Title,
        string? Description,
        DateTime? DueAt)
    {
    }
}
