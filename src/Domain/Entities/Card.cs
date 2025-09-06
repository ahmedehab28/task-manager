using Domain.Entities.Common;

namespace Domain.Entities
{
    public class Card : BaseAuditableEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Position { get; set; }
        public DateTime? DueAt { get; set; }
        public Guid CardListId { get; set; }

        public CardList CardList { get; private set; } = null!;
        public IList<CardMember> Members { get; private set; } = [];
    }
}
