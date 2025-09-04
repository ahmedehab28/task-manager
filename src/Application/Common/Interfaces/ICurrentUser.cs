
namespace Application.Common.Interfaces
{
    public interface ICurrentUser
    {
        Guid Id { get; }
        IReadOnlyList<string>? Roles { get; }
    }
}
