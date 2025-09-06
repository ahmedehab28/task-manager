namespace Application.List.DTOs
{
    public record ListDto(
        Guid Id,
        Guid BoardId,
        string Title,
        decimal Position);
}
