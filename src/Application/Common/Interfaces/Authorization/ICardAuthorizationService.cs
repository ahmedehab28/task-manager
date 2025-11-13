namespace Application.Common.Interfaces.Authorization
{
    public interface ICardAuthorizationService
    {
        Task<bool> CanAssignMembersToCardAsync(Guid cardId, Guid userId, CancellationToken cancellationToken);
    }
}
