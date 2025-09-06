using Domain.Entities.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public sealed class Board : BaseAuditableEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public BoardType BoardType { get; set; }
        public Guid ProjectId { get; set; }

        public Project Project { get; private set; } = null!;
        public IList<CardList> CardLists { get; private set; } = [];
    }
}
