using Domain.Entities.Common;

namespace Domain.Entities
{
    public class CardList : BaseAuditableEntity
    {
        public string Title { get; set; } = string.Empty;
        public decimal Position { get; set; }

        public Guid BoardId { get; set; }

        public Board Board { get; private set; } = null!;
        public IList<Card> Cards { get; private set; } = [];
    }
}
