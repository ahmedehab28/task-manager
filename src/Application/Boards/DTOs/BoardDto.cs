namespace Application.Boards.DTOs
{
    public record BoardDto (
        Guid Id,
        string Title,
        string? Description,
        DateTime CreatedAt);
}
