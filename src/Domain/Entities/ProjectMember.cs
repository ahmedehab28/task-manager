
using Domain.Entities.Common.Enums;

namespace Domain.Entities
{
    public class ProjectMember
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public ProjectRole Role { get; set; }
        public Project Project { get; set; } // nav
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    }
}
