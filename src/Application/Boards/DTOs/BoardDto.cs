namespace Application.Boards.DTOs
{
    public record BoardDto (
        Guid Id,
        IList<BoardListsDto> Lists);
}
