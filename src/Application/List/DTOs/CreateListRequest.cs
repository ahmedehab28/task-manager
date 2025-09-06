namespace Application.List.DTOs
{
    public record CreateListRequest(
        Guid BoardId,
        string Title,
        decimal Position);
}
