using Domain.Entities;
using Domain.Enums;
using FluentAssertions;

namespace Domain.Tests.Entities.Projects
{
    public class ProjectTests
    {
        [Fact]
        public void CreateWithDefaultInbox_Should_SetTitleAndDescription()
        {
            // Arrange
            string title = "Project Title";
            string description = "Project Description";

            // Act
            var project = Project.CreateWithDefaultInbox(title, description, Guid.NewGuid());

            // Assert
            project.Title.Should().Be(title);
            project.Description.Should().Be(description);
        }

        [Fact]
        public void CreateWithDefaultInbox_Should_AddToProjectMembersWithOwnerRole()
        {
            // Arrange
            string title = "Project Title";
            string description = "Project Description";
            Guid ownerId = Guid.NewGuid();

            // Act
            var project = Project.CreateWithDefaultInbox(title, description, ownerId);

            // Assert
            project.Members.Should().HaveCount(1);
            project.Members.Should().ContainSingle(m =>
                m.UserId == ownerId && m.Role == ProjectRole.Owner);
        }

        [Fact]
        public void CreateWithDefaultInbox_Should_CreateDefaultInbox()
        {
            // Arrange
            string title = "Project Title";
            string description = "Project Description";
            Guid ownerId = Guid.NewGuid();

            // Act
            var project = Project.CreateWithDefaultInbox(title, description, ownerId);

            // Assert
            project.Boards.Should().HaveCount(1);

            var inboxBoard = project.Boards.Should()
                .Contain(b => b.Title == "Inbox" && b.BoardType == BoardType.Inbox)
                .Subject;

            inboxBoard.CardLists.Should().HaveCount(1);
            inboxBoard.CardLists.Should().Contain(cl => cl.Title == "Inbox");
        }
    }
}
