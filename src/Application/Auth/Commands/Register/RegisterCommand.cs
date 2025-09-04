
using Application.Auth.DTOs;
using Application.Common.Models;
using MediatR;

namespace Application.Auth.Commands.Register
{
    public record RegisterCommand(
        string Email,
        string Username,
        string FirstName,
        string LastName,
        string Password,
        string ConfirmPassword) : IRequest<Result<RegisterResult>>;
}
