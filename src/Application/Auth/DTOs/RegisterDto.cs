
namespace Application.Auth.DTOs
{
    public record RegisterDto(
        string Email,
        string Username,
        string FirstName,
        string LastName, 
        string Password, 
        string ConfirmPassword);
}
