using Domain.Entities.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Project : BaseAuditableEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public IList<Board> Boards { get; private set; } = [];

        public IList<ProjectMember> Members { get; private set; } = [];

        public static Project CreateWithDefaultInbox(string title, string? description, Guid ownerId)
        {
            var project = new Project
            {
                Title = title,
                Description = description
            };

            project.Members.Add(new ProjectMember
            {
                ProjectId = project.Id,
                UserId = ownerId,
                Role = ProjectRole.Owner
            });

            var inboxBoard = new Board { Title = "Inbox", ProjectId = project.Id , BoardType=BoardType.Inbox };
            var inboxList = new CardList { Title = "Inbox", BoardId = inboxBoard.Id };
            inboxBoard.CardLists.Add(inboxList);
            project.Boards.Add(inboxBoard);

            return project;
        }

    }
}
