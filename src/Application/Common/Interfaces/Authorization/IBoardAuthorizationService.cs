namespace Application.Common.Interfaces.Authorization
{
    public interface IBoardAuthorizationService
    {
        Task<bool> CanAccessBoardAsync(Guid projectId, Guid boardId, Guid userId, CancellationToken cancellation);
    }
}
