using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Application.Projects.Commands.UpdateProject;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Application.Tests.Projects.Commands.UpdateProject
{
    public class UpdateProjectHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _context = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly Mock<IProjectAuthorizationService> _authService = new();
        private readonly UpdateProjectHandler _handler;

        public UpdateProjectHandlerTests()
        {
           _handler = new UpdateProjectHandler(_context.Object, _currentUser.Object, _authService.Object);
            _currentUser
                .Setup(x => x.Id)
                .Returns(Guid.NewGuid());
        }

        [Fact]
        public async Task Handler_Should_ThrowNotFoundException_When_UserIsNotAdmin()
        {
            // Arrange
            _authService
                .Setup(x => x.IsProjectAdminAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var cmd = new UpdateProjectCommand(Guid.NewGuid(), "Title", "Description");

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(cmd, CancellationToken.None))
                .Should()
                .ThrowAsync<NotFoundException>();

            _context.Verify(x => x.Projects.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()), Times.Never);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            _authService.Verify(x => x.IsProjectAdminAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handler_Should_ThrowNotFoundException_When_ProjectIsDeletedAfterBeingAuthorized()
        {
            // Arrange
            _authService
                .Setup(x => x.IsProjectAdminAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _context
                .Setup(x => x.Projects.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                .Returns(null!);

            var cmd = new UpdateProjectCommand(Guid.NewGuid(), "Title", "Description");

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(cmd, CancellationToken.None))
                .Should()
                .ThrowAsync<NotFoundException>();

            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            _authService.Verify(x => x.IsProjectAdminAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.Projects.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handler_Should_UpdateFields_When_UserIsAdmin()
        {
            // Arrange
            var projectId = Guid.NewGuid();

            string oldTitle = "Old Title";
            string oldDesc = "Old Description";
            var project = new Project
            {
                Title = oldTitle,
                Description = oldDesc
            };

            _authService
                .Setup(x => x.IsProjectAdminAsync(projectId, It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _context
                .Setup(x => x.Projects.FindAsync(new object[] { projectId }, It.IsAny<CancellationToken>()))
                .ReturnsAsync(project);

            string newTitle = "New Title";
            string newDesc = "New Description";
            var cmd = new UpdateProjectCommand(projectId, newTitle, newDesc);

            // Act
            await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            project.Title.Should().Be(newTitle);
            project.Description.Should().Be(newDesc);

            _authService.Verify(x => x.IsProjectAdminAsync(projectId, It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.Projects.FindAsync(new object[] { projectId }, It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handler_Should_Not_UpdateTitleOnly_When_OtherFieldsAreNull()
        {
            // Arrange
            var projectId = Guid.NewGuid();

            string oldTitle = "Old Title";
            string oldDesc = "Old Description";
            var project = new Project
            {
                Title = oldTitle,
                Description = oldDesc
            };

            _authService
                .Setup(x => x.IsProjectAdminAsync(projectId, It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _context
                .Setup(x => x.Projects.FindAsync(new object[] { projectId }, It.IsAny<CancellationToken>()))
                .ReturnsAsync(project);

            string newTitle = "New Title";
            string newDesc = null!;
            var cmd = new UpdateProjectCommand(projectId, newTitle, newDesc);

            // Act
            await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            project.Title.Should().Be(newTitle);
            project.Description.Should().Be(oldDesc);

            _authService.Verify(x => x.IsProjectAdminAsync(projectId, It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.Projects.FindAsync(new object[] { projectId }, It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handler_Should_UpdateDescriptionOnly_When_OtherFieldsAreNull()
        {
            // Arrange
            var projectId = Guid.NewGuid();

            string oldTitle = "Old Title";
            string oldDesc = "Old Description";
            var project = new Project
            {
                Title = oldTitle,
                Description = oldDesc
            };

            _authService
                .Setup(x => x.IsProjectAdminAsync(projectId, It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _context
                .Setup(x => x.Projects.FindAsync(new object[] { projectId }, It.IsAny<CancellationToken>()))
                .ReturnsAsync(project);

            string newTitle = null!;
            string newDesc = "New Description"!;
            var cmd = new UpdateProjectCommand(projectId, newTitle, newDesc);

            // Act
            await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            project.Title.Should().Be(oldTitle);
            project.Description.Should().Be(newDesc);

            _authService.Verify(x => x.IsProjectAdminAsync(projectId, It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.Projects.FindAsync(new object[] { projectId }, It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
