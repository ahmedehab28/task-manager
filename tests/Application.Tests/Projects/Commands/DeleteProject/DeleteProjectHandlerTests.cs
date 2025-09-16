using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Application.Projects.Commands.DeleteProject;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Application.Tests.Projects.Commands.DeleteProject
{
    public class DeleteProjectHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _context = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly Mock<IProjectAuthorizationService> _authService = new();
        private readonly DeleteProjectHandler _handler;

        public DeleteProjectHandlerTests()
        {
            _handler = new DeleteProjectHandler(_context.Object, _currentUser.Object, _authService.Object);
        }

        [Fact]
        public async Task Handler_Should_DeleteProject_WhenConditionsAreMet()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var fakeProject = new Project();

            _currentUser
                .Setup(x => x.Id)
                .Returns(userId);

            _authService
                .Setup(x => x.IsProjectOwnerAsync(projectId, userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _context
                .Setup(x => x.Projects.FindAsync(projectId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeProject);

            Project removedProject = null!;
            _context
                .Setup(x => x.Projects.Remove(It.IsAny<Project>()))
                .Callback<Project>(p => removedProject = p);


            var cmd = new DeleteProjectCommand(projectId);

            // Act
           await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            _authService.Verify(x => x.IsProjectOwnerAsync(projectId, userId, It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.Projects.FindAsync(projectId, It.IsAny<CancellationToken>()), Times.Once);
            removedProject.Should().BeSameAs(fakeProject);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handler_Should_ThrowNotFoundException_When_IsProjectOwnerFails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();

            _currentUser
                .Setup(x => x.Id)
                .Returns(userId);

            _authService
                .Setup(x => x.IsProjectOwnerAsync(projectId, userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var cmd = new DeleteProjectCommand(projectId);

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(cmd, CancellationToken.None))
                .Should()
                .ThrowAsync<NotFoundException>();

            _context.Verify(x => x.Projects.FindAsync(projectId, userId, It.IsAny<CancellationToken>()), Times.Never);
            _context.Verify(x => x.Projects.Remove(It.IsAny<Project>()), Times.Never);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            _authService.Verify(x => x.IsProjectOwnerAsync(projectId, userId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handler_Should_ThrowNotFoundException_When_ProjectIsDeletedAfterBeingAuthorized()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();

            _currentUser
                .Setup(x => x.Id)
                .Returns(userId);

            _authService
                .Setup(x => x.IsProjectOwnerAsync(projectId, userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _context
                .Setup(x => x.Projects.FindAsync(projectId, It.IsAny<CancellationToken>()))
                .Returns(null!);

            var cmd = new DeleteProjectCommand(projectId);
            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(cmd, CancellationToken.None))
                .Should()
                .ThrowAsync<NotFoundException>();

            _context.Verify(x => x.Projects.Remove(It.IsAny<Project>()), Times.Never);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            _authService.Verify(x => x.IsProjectOwnerAsync(projectId, userId, It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.Projects.FindAsync(projectId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
