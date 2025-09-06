using Application.List.DTOs;
using MediatR;

namespace Application.List.Commands.CreateList
{
    public record CreateListCommand(
        Guid BoardId,
        string Title,
        decimal Position) : IRequest<ListDto>
    {
    }
}
