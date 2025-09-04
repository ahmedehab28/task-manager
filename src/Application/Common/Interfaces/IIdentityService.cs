
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
        Task<Result<LoginResult>> LoginAsync(string email, string password);
        Task<Result<RegisterResult>> RegisterAsync(RegisterRequest registerRequest);
        Task<Result> ConfirmEmailAsync(Guid userId, string token);
    }
}
