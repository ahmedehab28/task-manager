using Application.Boards.Commands.UpdateBoard;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Application.Tests.Boards.Commands.UpdateBoard
{
    public class UpdateBoardHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _context = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly Mock<IAppAuthorizationService> _authService = new();
        private readonly UpdateBoardHandler _handler;

        public UpdateBoardHandlerTests()
        {
            _handler = new UpdateBoardHandler(_context.Object, _currentUser.Object, _authService.Object);
            _currentUser
                .Setup(x => x.Id)
                .Returns(It.IsAny<Guid>());
        }

        [Fact]
        public async Task Handler_Should_ThrowNotFoundException_When_UserCannotAccessBoard()
        {
            // Arrange
            _authService
                .Setup(x => x.CanAccessBoardAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var cmd = new UpdateBoardCommand(Guid.NewGuid(), "Title", "Description");

            // Act & Assert
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

            _context.Verify(x => x.Boards.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()), Times.Never);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handler_Should_ThrowNotFoundException_When_BoardIsDeletedAfterBeingAuthorized()
        {
            // Arrange
            _authService
                .Setup(x => x.CanAccessBoardAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _context
                .Setup(x => x.Boards.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                .Returns(null!);

            var cmd = new UpdateBoardCommand(Guid.NewGuid(), "Title", "Description");

            // Act & Assert
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
            _context.Verify(x => x.Boards.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()), Times.Once);

            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handler_Should_UpdateFields_When_UserIsAdminAndFieldsAreProvided()
        {
            // Arrange
            Guid boardId = Guid.NewGuid();
            string oldTitle = "Old Title";
            string oldDesc = "Old Description";

            Board board = new()
            {
                Id = boardId,
                Title = oldTitle,
                Description = oldDesc,
            };

            _authService
                .Setup(x => x.CanAccessBoardAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _context
                .Setup(x => x.Boards.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(board);

            _context
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            string newTitle = "New Title";
            string newDescription = "New Description";
            var cmd = new UpdateBoardCommand(boardId, newTitle, newDescription);

            // Act
            await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            board.Id.Should().Be(boardId);
            board.Title.Should().Be(newTitle);
            board.Description.Should().Be(newDescription);

            _authService
                .Verify(x =>
                    x.CanAccessBoardAsync(
                        It.IsAny<EntityOperations>(),
                        It.IsAny<Guid>(),
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.Boards.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handler_Should_Not_UpdateTitle_When_TitleIsNull()
        {
            // Arrange
            Guid boardId = Guid.NewGuid();
            string oldTitle = "Old Title";
            string oldDesc = "Old Description";

            Board board = new()
            {
                Id = boardId,
                Title = oldTitle,
                Description = oldDesc,
            };

            _authService
                .Setup(x => x.CanAccessBoardAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _context
                .Setup(x => x.Boards.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(board);

            _context
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            string newTitle = null!;
            string newDescription = "New Description";
            var cmd = new UpdateBoardCommand(boardId, newTitle, newDescription);

            // Act
            await _handler.Handle(cmd, CancellationToken.None);

            board.Id.Should().Be(boardId);
            board.Title.Should().Be(oldTitle);
            board.Description.Should().Be(newDescription);

            _authService
                .Verify(x =>
                    x.CanAccessBoardAsync(
                        It.IsAny<EntityOperations>(),
                        It.IsAny<Guid>(),
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.Boards.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handler_Should_Not_UpdateDescription_When_DescriptionIsNull()
        {
            // Arrange
            Guid boardId = Guid.NewGuid();
            string oldTitle = "Old Title";
            string oldDesc = "Old Description";

            Board board = new()
            {
                Id = boardId,
                Title = oldTitle,
                Description = oldDesc,
            };

            _authService
                .Setup(x => x.CanAccessBoardAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _context
                .Setup(x => x.Boards.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(board);

            _context
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            string newTitle = "New Title";
            string newDescription = null!;
            var cmd = new UpdateBoardCommand(boardId, newTitle, newDescription);

            // Act
            await _handler.Handle(cmd, CancellationToken.None);

            board.Id.Should().Be(boardId);
            board.Title.Should().Be(newTitle);
            board.Description.Should().Be(oldDesc);

            _authService
                .Verify(x =>
                    x.CanAccessBoardAsync(
                        It.IsAny<EntityOperations>(),
                        It.IsAny<Guid>(),
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.Boards.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
