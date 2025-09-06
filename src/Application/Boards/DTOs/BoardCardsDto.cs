namespace Application.Boards.DTOs
{
    public record BoardCardsDto(
        Guid Id,
        Guid BoardId,
        Guid ListId,
        string Title,
        string? Description,
        DateTime? DueAt,
        decimal Position);
}
