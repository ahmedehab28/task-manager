using Application.Auth.DTOs;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IApplicationDbContext _context;
        private readonly ITokenGenerator _tokenGenerator;

        public IdentityService(
            IApplicationDbContext context,
            ITokenGenerator tokenGenerator)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
        }
        public async Task<Result<LoginResult>> LoginAsync(string email, string password, CancellationToken cancellationToken)
        {
            var normalizedEmail = email.Trim().ToLower();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail, cancellationToken);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return Result<LoginResult>.Failure(new[] { "Invalid email or password." });

            var token = _tokenGenerator.GenerateToken(user.Id, email, user.UserName);
            return Result<LoginResult>.Success(new LoginResult(Token: token));
        }

        public async Task<Result<RegisterResult>> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellation)
        {
            var normalizedEmail = registerRequest.Email.Trim().ToLower();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail);
            if (user != null)
                return Result<RegisterResult>.Failure(new[] { "Email already used." });

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password, workFactor: 12);

            var newUser = new ApplicationUser
            {
                Email = normalizedEmail,
                UserName = registerRequest.UserName,
                PasswordHash = hashedPassword,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createUserResult = _context.Users.Add(newUser);

            // Handle email confirmation token.


            await _context.SaveChangesAsync(cancellation);
            return Result<RegisterResult>.Success(new RegisterResult(Id: newUser.Id));
        }

        public async Task<Result> ConfirmEmailAsync(Guid userId, string token, CancellationToken cancellation)
        {
            throw new NotImplementedException("Not implemented yet");
        }
    }
}
