using Application.List.DTOs;
using MediatR;

namespace Application.List.Queries.GetListById
{
    public record GetListByIdQuery(
        Guid ListId ) : IRequest<ListDto>
    {
    }
}
