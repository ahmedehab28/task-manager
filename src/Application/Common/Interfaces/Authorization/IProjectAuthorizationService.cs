namespace Application.Common.Interfaces.Authorization
{
    public interface IProjectAuthorizationService
    {
        Task<bool> IsProjectOwnerAsync(Guid projectId, Guid userId, CancellationToken cancellationToken = default);
        Task<bool> IsProjectAdminAsync(Guid projectId, Guid userId, CancellationToken cancellationToken = default);
        Task<bool> IsProjectMemberAsync(Guid projectId, Guid userId, CancellationToken cancellationToken = default);
    }
}
