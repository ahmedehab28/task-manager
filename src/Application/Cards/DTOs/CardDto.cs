namespace Application.Cards.DTOs
{
    public record CardDto(
        Guid Id,
        Guid ListId,
        string Title,
        string? Description,
        DateTime? DueAt,
        decimal Position,
        IList<CardMemberDto> CardMembers);
}
