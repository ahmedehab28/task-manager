
namespace Application.Projects.DTOs
{
    public record ProjectDto(
        Guid Id,
        string Title,
        string? Description = null);
}
