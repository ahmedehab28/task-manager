using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Application.List.Commands.CreateList;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Application.Tests.Lists.Commands.CreateList
{
    public class CreateListHandlerTest
    {
        private readonly Mock<IApplicationDbContext> _context = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly Mock<IAppAuthorizationService> _authService = new();
        private readonly CreateListHandler _handler;

        public CreateListHandlerTest()
        {
            _handler = new CreateListHandler(_context.Object, _currentUser.Object, _authService.Object);
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

            var cmd = new CreateListCommand(Guid.NewGuid(), "Title", 1000);

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

            _context.Verify(x => x.CardLists.AddAsync(It.IsAny<CardList>(), It.IsAny<CancellationToken>()), Times.Never);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handler_Should_CreateList_When_UserCanAccessBoard()
        {
            // Arrange
            Guid boardId = Guid.NewGuid();
            string title = "Title";
            decimal position = 1000m;

            _authService
                .Setup(x => x.CanAccessBoardAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            CardList capturedList = null!;
                _context
                    .Setup(x => x.CardLists.AddAsync(It.IsAny<CardList>(), It.IsAny<CancellationToken>()))
                    .Callback<CardList, CancellationToken>((cl, _) => capturedList = cl);

            var cmd = new CreateListCommand(boardId, title, position);

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            capturedList.BoardId.Should().Be(boardId);
            capturedList.Title.Should().Be(title);
            capturedList.Position.Should().Be(position);

            result.Id.Should().Be(capturedList.Id);
            result.BoardId.Should().Be(boardId);
            result.Title.Should().Be(title);
            result.Position.Should().Be(position);

            _authService
                .Verify(x =>
                    x.CanAccessBoardAsync(
                        It.IsAny<EntityOperations>(),
                        It.IsAny<Guid>(),
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.CardLists.AddAsync(It.IsAny<CardList>(), It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
