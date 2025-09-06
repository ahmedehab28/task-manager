
namespace Application.Cards.DTOs
{
    public record CardDetailsDto(
        Guid Id,
        Guid ListId,
        string Title,
        string? Description,
        DateTime? DueAt,
        decimal Position);
}
