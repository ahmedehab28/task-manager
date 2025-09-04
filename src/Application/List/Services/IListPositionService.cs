namespace Application.List.Services
{
    public interface IListPositionService
    {
        Task<decimal> GetNewListPositionAsync(Guid boardId, CancellationToken cancellationToken = default);
        Task<decimal> GetMovedListPositionAsync(Guid boardId, Guid? prevListId, Guid? nextListId, Guid listId, CancellationToken cancellationToken = default);
    }
}
