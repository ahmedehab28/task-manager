using Application.List.DTOs;
using MediatR;

namespace Application.List.Commands.CreateList
{
    public record CreateListCommand(
        Guid ProjectId,
        Guid BoardId,
        string Title) : IRequest<ListDto>
    {
    }
}
