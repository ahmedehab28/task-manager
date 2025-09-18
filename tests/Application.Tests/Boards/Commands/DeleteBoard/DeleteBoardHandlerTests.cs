using Application.Boards.Commands.DeleteBoard;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Application.Tests.Boards.Commands.DeleteBoard
{
    public class DeleteBoardHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _context = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly Mock<IAppAuthorizationService> _authService = new();
        private readonly DeleteBoardHandler _handler;

        public DeleteBoardHandlerTests()
        {
            _handler = new DeleteBoardHandler(_context.Object, _currentUser.Object, _authService.Object);
            _currentUser
                .Setup(x => x.Id)
                .Returns(It.IsAny<Guid>());
        }

        [Fact]
        public async Task Handler_Should_DeleteBoard_WhenUserCanAccessBoard()
        {
            // Arrange
            Board fakeBoard = new ();

            _authService
                .Setup(x => x.CanAccessBoardAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _context
                .Setup(x => x.Boards.FindAsync(new object[] { It.IsAny<Guid>() }, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeBoard);

            Board removedBoard = null!;
            _context
                .Setup(x => x.Boards.Remove(It.IsAny<Board>()))
                .Callback<Board>(b => removedBoard = b);

            _context
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var cmd = new DeleteBoardCommand(It.IsAny<Guid>());

            // Act
            await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            removedBoard.Should().BeSameAs(fakeBoard);

            _authService
                .Verify(x =>
                    x.CanAccessBoardAsync(
                        It.IsAny<EntityOperations>(),
                        It.IsAny<Guid>(),
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.Boards.FindAsync(new object[] { It.IsAny<Guid>() }, It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.Boards.Remove(It.IsAny<Board>()), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handler_Should_ThrowNotFoundException_When_UserCannotAccessBoard()
        {
            // Arrange
            _authService
                .Setup(x => x.CanAccessBoardAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var cmd = new DeleteBoardCommand(It.IsAny<Guid>());

            // Act
            await FluentActions.Invoking(() => _handler.Handle(cmd, CancellationToken.None))
                .Should()
                .ThrowAsync<NotFoundException>();

            _authService
                .Verify(x =>
                    x.CanAccessBoardAsync(
                        It.IsAny<EntityOperations>(),
                        It.IsAny<Guid>(),
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()), Times.Once);

            _context.Verify(x => x.Boards.FindAsync(new object[] { It.IsAny<Guid>() }, It.IsAny<CancellationToken>()), Times.Never);
            _context.Verify(x => x.Boards.Remove(It.IsAny<Board>()), Times.Never);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handler_Should_ThrowNotFoundException_When_BoardIsDeletedAfterAuthorization()
        {
            // Arrange
            _authService
                .Setup(x => x.CanAccessBoardAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _context
                .Setup(x => x.Boards.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .Returns(null!);

            var cmd = new DeleteBoardCommand(It.IsAny<Guid>());

            // Act
            await FluentActions.Invoking(() => _handler.Handle(cmd, CancellationToken.None))
                .Should()
                .ThrowAsync<NotFoundException>();

            _authService
                .Verify(x =>
                    x.CanAccessBoardAsync(
                        It.IsAny<EntityOperations>(),
                        It.IsAny<Guid>(),
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.Boards.FindAsync(new object[] { It.IsAny<Guid>() }, It.IsAny<CancellationToken>()), Times.Once);

            _context.Verify(x => x.Boards.Remove(It.IsAny<Board>()), Times.Never);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
