using Application.Cards.DTOs;
using Application.Common.Models;
using MediatR;

namespace Application.Cards.Commands.UpdateCard
{
    public record UpdateCardCommand(
        Guid CardId,
        Optional<string?> Title = default,
        Optional<string?> Description = default,
        Optional<DateTime?> DueAt = default,
        Optional<Guid?> TargetListId = default,
        Optional<decimal?> Position = default) : IRequest<CardDetailsDto>;
}
