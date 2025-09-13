using Application.Auth.DTOs;
using Application.Common.Models;

namespace Application.Common.Interfaces
{
    public record RegisterRequest(
        string Email,
        string UserName,
        string FirstName,
        string LastName,
        string Password
    );
    public interface IIdentityService
    {
        Task<Result<LoginResult>> LoginAsync(string email, string password, CancellationToken cancellation);
        Task<Result<RegisterResult>> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellation);
        Task<Result> ConfirmEmailAsync(Guid userId, string token, CancellationToken cancellation);
    }
}
