namespace Application.Boards.DTOs
{
    public record BoardDetailsDto(
        Guid Id,
        Guid ProjectId,
        string Title,
        string? Description)
    {
    }
}
