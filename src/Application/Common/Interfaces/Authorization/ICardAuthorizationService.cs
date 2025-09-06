using System.Threading.Tasks;

namespace Application.Common.Interfaces.Authorization
{
    public interface ICardAuthorizationService
    {
        Task<bool> CanMoveCardAsync(Guid listId, Guid cardId, Guid userId, CancellationToken cancellationToken);
    }
}
