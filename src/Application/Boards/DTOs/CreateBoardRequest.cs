
namespace Application.Boards.DTOs
{
    public record CreateBoardRequest (
        Guid ProjectId,
        string Title,
        string? Description);
}
