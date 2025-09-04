
namespace Application.Cards.Services
{
    public interface ICardPositionService
    {
        Task<decimal> GetNewCardPositionAsync(Guid boardId, Guid? listId, CancellationToken cancellationToken = default);
        Task<decimal> GetMovedCardPositionAsync(Guid boardId, Guid? targetListId, Guid? prevCardId, Guid? nextCardId, Guid CardId, CancellationToken cancellationToken = default);
    }
}
