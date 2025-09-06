namespace Application.List.DTOs
{
    public record UpdateListRequest(
        Guid? BoardId,
        string? Title,
        decimal? Position)
    {
    }
}
