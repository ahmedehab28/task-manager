using Domain.Entities.Common;

namespace Domain.Entities
{
    public class Project : BaseAuditableEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public IList<Board> Boards { get; private set; } = new List<Board>();
        public IList<ProjectMember> Members { get; private set; } = new List<ProjectMember>();
    }
}
