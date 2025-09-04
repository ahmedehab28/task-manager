using Application.Auth.Commands.Login;
using Application.Auth.Commands.Register;
using Application.Auth.DTOs;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/[controller]")]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator) 
        {
            _mediator = mediator;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var result = await _mediator.Send(new LoginCommand(request.Email, request.Password));
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return Ok(new LoginResponseDto(result.Value!.Token));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var result = await _mediator.Send(new RegisterCommand(
                Email: request.Email,
                Username: request.Username,
                FirstName: request.FirstName,
                LastName: request.LastName,
                Password: request.Password,
                ConfirmPassword: request.ConfirmPassword
            ));
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return Ok(new RegisterResponseDto(result.Value!.Id));
        }
    }
}
