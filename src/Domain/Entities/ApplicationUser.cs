using Domain.Entities.Common;

namespace Domain.Entities
{
    public class ApplicationUser : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string? AvatarUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastActiveAt { get; set; }

        public bool IsActive { get; set; } = true;

        public string? TimeZone { get; set; }

        public string? Locale { get; set; }

        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        //public string? SecurityStamp { get; set; }
        //public string? ConcurrencyStamp { get; set; }

        public IList<ProjectMember> ProjectMemberships { get; private set; } = [];
        public IList<CardMember> AssignedCards { get; private set; } = [];
    }
}
