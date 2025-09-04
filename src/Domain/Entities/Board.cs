using Domain.Entities.Common;

namespace Domain.Entities
{
    public sealed class Board : BaseAuditableEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }

        public IList<Card> Cards { get; set; } = [];
        public IList<CardList> Lists { get; set; } = [];
        public Guid ProjectId { get; set; } // FK to projects table
        public Project Project { get; set; } // Nav so we can access roperties in a Project

    }
}
