﻿using Domain.Enums;

namespace Domain.Entities
{
    public class ProjectMember
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public ProjectRole Role { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public Project Project { get; private set; } = null!;

    }
}
