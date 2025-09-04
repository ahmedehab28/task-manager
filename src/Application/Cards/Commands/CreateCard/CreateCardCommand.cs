using Application.Cards.DTOs;
using MediatR;

namespace Application.Cards.Commands.CreateCard
{
    public record CreateCardCommand(
        Guid ProjectId,
        Guid BoardId,
        Guid? ListId,
        string Title) : IRequest<CardDto>
    {
    }
}
