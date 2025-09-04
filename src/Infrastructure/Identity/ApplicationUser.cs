
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? AvatarUrl { get; set; }

        public DateTime CreatedAt { get; set; } 
        public DateTime? LastActiveAt { get; set; }

        public bool IsActive { get; set; } = true;

        public string? TimeZone { get; set; }
        public string? Locale { get; set; }
    }
}
