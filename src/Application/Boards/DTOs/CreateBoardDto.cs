
namespace Application.Boards.DTOs
{
    public record CreateBoardDto (
        Guid ProjectId,
        string Title,
        string? Description);
}
