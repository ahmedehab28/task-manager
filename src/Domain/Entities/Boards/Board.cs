using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Boards
{
    public sealed class Board : BaseEntity
    {
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

        public Board(string title, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required");
            Title = title;
            Description = description;
        }

    }
}
