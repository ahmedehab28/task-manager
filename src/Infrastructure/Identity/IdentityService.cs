
using Application.Auth.DTOs;
using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenGenerator _tokenGenerator;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            ITokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }
        public async Task<Result<LoginResult>> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
                return Result<LoginResult>.Failure(new[] { "Invalid email or password." });
            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenGenerator.GenerateToken(user.Id, email, user.UserName!, roles.ToArray());
            return Result<LoginResult>.Success(new LoginResult(Token: token));
        }

        public async Task<Result<RegisterResult>> RegisterAsync(RegisterRequest registerRequest)
        {
            var user = await _userManager.FindByEmailAsync(registerRequest.Email);
            if (user != null)
                return Result<RegisterResult>.Failure(new[] { "Email already used." });

            var newUser = new ApplicationUser
            {
                Email = registerRequest.Email,
                UserName = registerRequest.UserName,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createUserResult = await _userManager.CreateAsync(newUser, registerRequest.Password);
            if (!createUserResult.Succeeded)
            {
                var errors = createUserResult.Errors.Select(e => e.Description);
                return Result<RegisterResult>.Failure(errors);
            }

            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            // Handle emailing the confirmation token.


            return Result<RegisterResult>.Success(new RegisterResult(Id: newUser.Id));
        }

        public async Task<Result> ConfirmEmailAsync(Guid userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return Result.Failure(new[] { "User not found." });
            var result = await _userManager.ConfirmEmailAsync(user, token);

            return result.Succeeded ?
                Result.Success() :
                Result.Failure(result.Errors.Select(e => e.Description));
        }
    }
}
