
using Application.Auth.DTOs;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Auth.Commands.Register
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, Result<RegisterResult>>
    {
        private readonly IIdentityService _identityService;
        public RegisterHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<Result<RegisterResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.RegisterAsync(new RegisterRequest(
                Email: request.Email,
                UserName: request.Username,
                FirstName: request.FirstName,
                LastName: request.LastName,
                Password: request.Password
            ), cancellationToken);
            return result;
        }
    }
}
