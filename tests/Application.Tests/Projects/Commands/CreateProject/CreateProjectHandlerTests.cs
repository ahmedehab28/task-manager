using Application.Common.Interfaces;
using Application.Projects.Commands.CreateProject;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Application.Tests.Projects.Commands.CreateProject
{
    
    public class CreateProjectHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _context = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly CreateProjectHandler _handler;
        public CreateProjectHandlerTests()
        {
            _handler = new CreateProjectHandler(_context.Object, _currentUser.Object);
        }
        [Fact]
        public async Task CreateProjectHandle_WithDefaults_ReturnsProjectDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _currentUser.Setup(x => x.Id).Returns(userId);

            var cmd = new CreateProjectCommand("Test Project", "Description");

            Project capturedProject = null!;
            _context.Setup(x => x.Projects.Add(It.IsAny<Project>()))
                      .Callback<Project>(p => capturedProject = p);

            _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            capturedProject.Should().NotBeNull();
            capturedProject.Title.Should().Be("Test Project");
            capturedProject.Description.Should().Be("Description");

            result.Id.Should().Be(capturedProject.Id);
            result.Title.Should().Be("Test Project");
            result.Description.Should().Be("Description");

            _context.Verify(x => x.Projects.Add(It.IsAny<Project>()), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
