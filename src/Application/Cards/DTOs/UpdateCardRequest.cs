using Application.Common.Models;

namespace Application.Cards.DTOs
{
    public record UpdateCardRequest(
        Guid CardId,
        Optional<string?> Title,
        Optional<string?> Description,
        Optional<DateTime?> DueAt,
        Optional<Guid?> TargetListId,
        Optional<decimal?> Position);
}
