namespace Application.Boards.DTOs
{
    public record BoardListsDto(
        Guid Id,
        Guid BoardId,
        string Title,
        decimal Position,
        IList<BoardCardsDto> Cards);
}
