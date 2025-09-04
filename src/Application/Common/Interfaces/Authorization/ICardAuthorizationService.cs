namespace Application.Common.Interfaces.Authorization
{
    public interface ICardAuthorizationService
    {
        Task<bool> CanCreateCardAsync(Guid ProjectId, Guid BoardId, Guid? ListId, Guid userId, CancellationToken cancellationToken);
        Task<bool> CanAccessCardAsync(Guid projectId, Guid boardId, Guid? listId, Guid cardId, Guid userId, CancellationToken cancellationToken);
        Task<Dictionary<Guid, Guid>> CanAccessCardsAsync(Guid projectId, Guid boardId, Guid? listId, IEnumerable<Guid> cardIds, Guid userId, CancellationToken cancellationToken);
    }
}
