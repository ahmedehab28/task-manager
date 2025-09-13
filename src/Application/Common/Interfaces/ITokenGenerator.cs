

namespace Application.Common.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateToken(Guid Id, string Email, string Username);
    }
}
