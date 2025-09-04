using Domain.Entities.Common;

namespace Domain.Entities
{
    public class Card : BaseAuditableEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Position { get; set; }
        public DateTime? DueAt { get; set; }
        public Guid BoardId { get; set; }    // FK to parent board (incase of null list)
        public Board Board { get; private set; }    // Nav
        public Guid? ListId { get; set; }    // FK to parent list (nullable for inbox cards)
        public CardList? List { get; private set; }    // Nav
        public IList<CardMember> Members { get; private set; } = new List<CardMember>();
    }
}
