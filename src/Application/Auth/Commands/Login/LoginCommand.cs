
using Application.Auth.DTOs;
using Application.Common.Models;
using MediatR;

namespace Application.Auth.Commands.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<Result<LoginResult>>;
}
