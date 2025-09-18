using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Projects.Queries.GetProjectById;
using Domain.Entities;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;

namespace Application.Tests.Projects.Queries.GetProjectById
{
    public class GetProjectByIdHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _context = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly GetProjectByIdHandler _handler;

        public GetProjectByIdHandlerTests()
        {
            _handler = new GetProjectByIdHandler(_context.Object, _currentUser.Object);
        }

        [Fact]
        public async Task Handler_Should_ThrowNotFoundException_When_MembershipIsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();

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
            var mockProjectMembers = members.BuildMockDbSet();

            _currentUser
                .Setup(x => x.Id)
                .Returns(userId);

            _context
                .Setup(x => x.Projects)
                .Returns(mockProjects.Object);

            _context
                .Setup(x => x.ProjectMembers)
                .Returns(mockProjectMembers.Object);

            var cmd = new GetProjectByIdQuery(projectOneId);

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(cmd, CancellationToken.None))
                .Should()
                .ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Handler_Should_ReturnProjectDto_When_MembershipIsFound()
        {
            // Arrange
            var userId = Guid.NewGuid();

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
                new () { UserId = userId, ProjectId = projectOneId },
                new () { UserId = Guid.NewGuid(), ProjectId = projectTwoId }
            ];

            var mockProjects = projects.BuildMockDbSet();
            var mockProjectMembers = members.BuildMockDbSet();

            _currentUser
                .Setup(x => x.Id)
                .Returns(userId);

            _context
                .Setup(x => x.Projects)
                .Returns(mockProjects.Object);

            _context
                .Setup(x => x.ProjectMembers)
                .Returns(mockProjectMembers.Object);

            var cmd = new GetProjectByIdQuery(projectOneId);

            // Act & Assert
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();

            result.Id.Should().Be(projectOneId);
            result.Title.Should().Be(projectOneTitle);
            result.Description.Should().Be(projectOneDescription);
        }
    }
}
