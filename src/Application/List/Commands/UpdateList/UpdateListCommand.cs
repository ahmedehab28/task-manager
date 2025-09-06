using Application.List.DTOs;
using MediatR;

namespace Application.List.Commands.UpdateList
{
    public record UpdateListCommand(
        Guid ListId,
        Guid? BoardId,
        string? Title,
        decimal? Position) : IRequest<ListDto>
    {
    }
}
