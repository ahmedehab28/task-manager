
namespace Application.Cards.DTOs
{
    public record CardDto(
        Guid Id,
        Guid BoardId,
        Guid? ListId,
        string Title,
        string? Description,
        decimal Position,
        DateTime? DueAt);
}
