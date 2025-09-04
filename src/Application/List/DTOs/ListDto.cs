namespace Application.List.DTOs
{
    public record ListDto(
        Guid BoardId,
        Guid ListId,
        string Title,
        decimal Position);
}
