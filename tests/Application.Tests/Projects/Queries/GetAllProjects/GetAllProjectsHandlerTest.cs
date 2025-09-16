using Application.Common.Interfaces;
using Application.Projects.Queries.GetAllProjects;
using Domain.Entities;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;

namespace Application.Tests.Projects.Queries.GetAllProjects
{
    public class GetAllProjectsHandlerTest
    {
        private readonly Mock<IApplicationDbContext> _context = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly GetAllProjectsHandler _handler;

        public GetAllProjectsHandlerTest()
        {
            _handler = new GetAllProjectsHandler(_context.Object, _currentUser.Object);
        }

        [Fact]
        public async Task Handler_Should_ReturnEmptyList_When_UserHasNoProjects()
        {
            // Arrange
            Guid userId = Guid.NewGuid();

            Guid projectOneId = Guid.NewGuid();
            string projectOneTitle = "Project One";
            string projectOneDescription = "Description One";

            Guid projectTwoId = Guid.NewGuid();
            string projectTwoTitle = "Project Two";
            string projectTwoDescription = "Description Two";

            List<Project> projects = [
                new () { Id = projectOneId, Title = projectOneTitle, Description = projectOneDescription },
                new () { Id = projectTwoId, Title = projectTwoTitle, Description = projectTwoDescription }
            ];

            List<ProjectMember> members = [
                new () { UserId = Guid.NewGuid(), ProjectId = projectOneId },
                new () { UserId = Guid.NewGuid(), ProjectId = projectTwoId }
            ];

            var mockProjects = projects.BuildMockDbSet();
            var mockMembers = members.BuildMockDbSet();

            _currentUser
                .Setup(x => x.Id)
                .Returns(userId);

            _context
                .Setup(x => x.Projects)
                .Returns(mockProjects.Object);

            _context
                .Setup(x => x.ProjectMembers)
                .Returns(mockMembers.Object);


            var cmd = new GetAllProjectsQuery();

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            result.Should().HaveCount(0);
        }

        [Fact]
        public async Task Handler_Should_ReturnProjectsList_When_UserHasProjects()
        {
            // Arrange
            Guid userId = Guid.NewGuid();

            Guid projectOneId = Guid.NewGuid();
            string projectOneTitle = "Project One";
            string projectOneDescription = "Description One";

            Guid projectTwoId = Guid.NewGuid();
            string projectTwoTitle = "Project Two";
            string projectTwoDescription = "Description Two";

            Guid projectThreeId = Guid.NewGuid();
            string projectThreeTitle = "Project Three";
            string projectThreeDescription = "Description Three";

            List<Project> projects = [
                new () { Id = projectOneId, Title = projectOneTitle, Description = projectOneDescription },
                new () { Id = projectTwoId, Title = projectTwoTitle, Description = projectTwoDescription },
                new () { Id = projectThreeId, Title = projectThreeTitle, Description = projectThreeDescription }
            ];

            List<ProjectMember> members = [
                new () { UserId = userId, ProjectId = projectOneId },
                new () { UserId = userId, ProjectId = projectTwoId },
                new () { UserId = Guid.NewGuid(), ProjectId = projectThreeId }
            ];

            var mockProjects = projects.BuildMockDbSet();
            var mockMembers = members.BuildMockDbSet();

            _currentUser
                .Setup(x => x.Id)
                .Returns(userId);

            _context
                .Setup(x => x.Projects)
                .Returns(mockProjects.Object);

            _context
                .Setup(x => x.ProjectMembers)
                .Returns(mockMembers.Object);


            var cmd = new GetAllProjectsQuery();

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            result.Select(p => p.Id).Should().BeEquivalentTo<Guid>([projectOneId, projectTwoId]);
            result.Select(p => p.Title).Should().BeEquivalentTo<string>([projectOneTitle, projectTwoTitle]);
            result.Select(p => p.Description).Should().BeEquivalentTo<string>([projectOneDescription, projectTwoDescription]);


        }
    }
}
