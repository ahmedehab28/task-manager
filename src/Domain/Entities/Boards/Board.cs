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
            ValidateTitle(title);
            Title = title;
            Description = description;
        }

        public void UpdateTitle(string newTitle)
        {
            ValidateTitle(newTitle);
            Title = newTitle;
        }

        public void UpdateDescription (string newDescription)
        {
            Description = newDescription;
        }

        public static void ValidateTitle(string boardTitle)
        {
            if (string.IsNullOrWhiteSpace(boardTitle))
                throw new ArgumentException("Title is required");
        }
    }
}
