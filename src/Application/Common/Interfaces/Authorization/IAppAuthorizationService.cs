using Application.Common.Enums;
using Domain.Entities;

namespace Application.Common.Interfaces.Authorization
{
    public interface IAppAuthorizationService
    {
        Task<bool> CanAccessProject(EntityOperations op, Guid projectId, Guid userId, CancellationToken cancellation);
        Task<bool> CanAccessBoardAsync(EntityOperations op, Guid boardId, Guid userId, CancellationToken cancellationToken);
        Task<Board?> GetBoardAsync(EntityOperations op, Guid boardId, Guid userId, CancellationToken cancellationToken);
        Task<bool> CanAccessListAsync(EntityOperations op, Guid listId, Guid userId, CancellationToken cancellationToken);
        Task<CardList?> GetListAsync(EntityOperations op, Guid listId, Guid userId, CancellationToken cancellationToken);
        Task<bool> CanAccessCardAsync(EntityOperations op, Guid cardId, Guid userId, CancellationToken cancellationToken);
        Task<Card?> GetCardAsync(EntityOperations op, Guid cardId, Guid userId, CancellationToken cancellationToken);
    }
}
