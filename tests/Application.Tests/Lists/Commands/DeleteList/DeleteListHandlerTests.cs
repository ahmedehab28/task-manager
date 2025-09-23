using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Application.List.Commands.DeleteList;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Application.Tests.Lists.Commands.DeleteList
{
    public class DeleteListHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _context = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly Mock<IAppAuthorizationService> _authService = new();
        private readonly DeleteListHandler _handler;

        public DeleteListHandlerTests()
        {
            _handler = new DeleteListHandler(_context.Object, _currentUser.Object, _authService.Object);
            _currentUser
                .Setup(x => x.Id)
                .Returns(It.IsAny<Guid>());
        }

        [Fact]
        public async Task Handler_Should_ThrowNotFoundException_When_User_CannotAccessList()
        {
            // Arrange
            _authService
                .Setup(x => x.GetListAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((CardList?) null);

            var cmd = new DeleteListCommand(Guid.NewGuid());

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(cmd, CancellationToken.None))
                .Should()
                .ThrowAsync<NotFoundException>();

            _authService
                .Verify(x =>
                    x.GetListAsync(
                        It.IsAny<EntityOperations>(),
                        It.IsAny<Guid>(),
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()), Times.Once);

            _context.Verify(x => x.CardLists.Remove(It.IsAny<CardList>()), Times.Never);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handler_Should_DeleteList_When_User_CanAcessList()
        {
            // Arrange
            Guid listId = Guid.NewGuid();
            Guid boardId = Guid.NewGuid();

            CardList fakeList = new();

            _authService
                .Setup(x => x.GetListAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeList);

            CardList deletedList = null!;
            _context
                .Setup(x => x.CardLists.Remove(It.IsAny<CardList>()))
                .Callback<CardList>(l => deletedList = l);

            _context
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var cmd = new DeleteListCommand(Guid.NewGuid());

            // Act
            await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            deletedList.Should().BeSameAs(fakeList);

            _authService
                .Verify(x =>
                    x.GetListAsync(
                        It.IsAny<EntityOperations>(),
                        It.IsAny<Guid>(),
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.CardLists.Remove(It.IsAny<CardList>()), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
