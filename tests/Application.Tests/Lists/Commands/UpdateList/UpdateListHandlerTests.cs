using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Application.List.Commands.UpdateList;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Application.Tests.Lists.Commands.UpdateList
{
    public class UpdateListHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _context = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly Mock<IAppAuthorizationService> _authService = new();
        private readonly UpdateListHandler _handler;

        public UpdateListHandlerTests()
        {
            _handler = new UpdateListHandler(_context.Object, _currentUser.Object, _authService.Object);
            _currentUser
                .Setup(x => x.Id)
                .Returns(It.IsAny<Guid>());
        }

        [Fact]
        public async Task Handler_Should_ThrowNotFoundException_When_UserCannotAccessList()
        {
            // Arrange
            _authService
                .Setup(x => x.GetListAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((CardList?)null);

            var cmd = new UpdateListCommand(Guid.NewGuid(), Guid.NewGuid(), "Title", 1000m);

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(cmd, CancellationToken.None))
                .Should()
                .ThrowAsync<NotFoundException>();

            _authService.Verify(x =>
                x.GetListAsync(
                    It.IsAny<EntityOperations>(),
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()),
                    Times.Once);

            _authService.Verify(x =>
                x.CanAccessBoardAsync(
                    It.IsAny<EntityOperations>(),
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()),
                    Times.Never);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handler_Should_ThrowNotFoundException_When_UserCannotAccessTargetBoard()
        {
            // Arrange
            var fakeList = new CardList();

            _authService
                .Setup(x => x.GetListAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeList);

            _authService
                .Setup(x => x.CanAccessBoardAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var cmd = new UpdateListCommand(Guid.NewGuid(), Guid.NewGuid(), "Title", 1000m);

            // Act
            await FluentActions.Invoking(() => _handler.Handle(cmd, CancellationToken.None))
                .Should()
                .ThrowAsync<NotFoundException>();

            _authService.Verify(x =>
                x.GetListAsync(
                    It.IsAny<EntityOperations>(),
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()),
                    Times.Once);
            _authService.Verify(x =>
                x.CanAccessBoardAsync(
                    It.IsAny<EntityOperations>(),
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()),
                    Times.Once);

            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handler_Should_MakeNoChanges_When_NonListIdFieldsAreNull()
        {
            // Arrange
            Guid oldBoardId = Guid.NewGuid();
            string oldTitle = "Old Title";
            decimal oldPosition = 1000m;
            var fakeList = new CardList
            {
                BoardId = oldBoardId,
                Title = oldTitle,
                Position = oldPosition,
            };

            _authService
                .Setup(x => x.GetListAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeList);

            var cmd = new UpdateListCommand(fakeList.Id, null, null, null);

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            result.Id.Should().Be(fakeList.Id);
            result.BoardId.Should().Be(oldBoardId);
            result.Title.Should().Be(oldTitle);
            result.Position.Should().Be(oldPosition);

            _authService.Verify(x =>
                x.GetListAsync(
                    It.IsAny<EntityOperations>(),
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()),
                    Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            _authService.Verify(x =>
                x.CanAccessBoardAsync(
                    It.IsAny<EntityOperations>(),
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()),
                    Times.Never);
        }

        [Fact]
        public async Task Handler_Should_UpdateTitleOnly_When_OnlyTitleIsProvidded()
        {
            // Arrange
            Guid oldBoardId = Guid.NewGuid();
            string oldTitle = "Old Title";
            decimal oldPosition = 1000m;
            var fakeList = new CardList
            {
                BoardId = oldBoardId,
                Title = oldTitle,
                Position = oldPosition,
            };

            _authService
                .Setup(x => x.GetListAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeList);

            var newTitle = "New Title";
            var cmd = new UpdateListCommand(fakeList.Id, null, newTitle, null);

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            result.Id.Should().Be(fakeList.Id);
            result.BoardId.Should().Be(oldBoardId);
            result.Title.Should().Be(newTitle);
            result.Position.Should().Be(oldPosition);

            _authService.Verify(x =>
                x.GetListAsync(
                    It.IsAny<EntityOperations>(),
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()),
                    Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            _authService.Verify(x =>
                x.CanAccessBoardAsync(
                    It.IsAny<EntityOperations>(),
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()),
                    Times.Never);
        }

        [Fact]
        public async Task Handler_Should_UpdatePositionAndBoardOnly_When_BothAreProvided()
        {
            // Arrange
            Guid oldBoardId = Guid.NewGuid();
            string oldTitle = "Old Title";
            decimal oldPosition = 1000m;
            var fakeList = new CardList
            {
                BoardId = oldBoardId,
                Title = oldTitle,
                Position = oldPosition,
            };

            _authService
                .Setup(x => x.GetListAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeList);

            _authService
                .Setup(x => x.CanAccessBoardAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var newBoardId = Guid.NewGuid();
            var newPosition = 2000m;
            var cmd = new UpdateListCommand(fakeList.Id, newBoardId, null, newPosition);

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            result.Id.Should().Be(fakeList.Id);
            result.BoardId.Should().Be(newBoardId);
            result.Title.Should().Be(oldTitle);
            result.Position.Should().Be(newPosition);

            _authService.Verify(x =>
                x.GetListAsync(
                    It.IsAny<EntityOperations>(),
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()),
                    Times.Once);
            _authService.Verify(x =>
                x.CanAccessBoardAsync(
                    It.IsAny<EntityOperations>(),
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()),
                    Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handler_Should_UpdateAllFields_When_AllAreProvidede()
        {
            // Arrange
            Guid oldBoardId = Guid.NewGuid();
            string oldTitle = "Old Title";
            decimal oldPosition = 1000m;
            var fakeList = new CardList
            {
                BoardId = oldBoardId,
                Title = oldTitle,
                Position = oldPosition,
            };

            _authService
                .Setup(x => x.GetListAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeList);

            _authService
                .Setup(x => x.CanAccessBoardAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            string newTitle = "New Title";
            var newBoardId = Guid.NewGuid();
            var newPosition = 2000m;
            var cmd = new UpdateListCommand(fakeList.Id, newBoardId, newTitle, newPosition);

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            result.Id.Should().Be(fakeList.Id);
            result.BoardId.Should().Be(newBoardId);
            result.Title.Should().Be(newTitle);
            result.Position.Should().Be(newPosition);

            _authService.Verify(x =>
                x.GetListAsync(
                    It.IsAny<EntityOperations>(),
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()),
                    Times.Once);
            _authService.Verify(x =>
                x.CanAccessBoardAsync(
                    It.IsAny<EntityOperations>(),
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()),
                    Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
