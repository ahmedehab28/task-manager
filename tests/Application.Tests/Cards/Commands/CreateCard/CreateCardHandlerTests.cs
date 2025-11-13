using Application.Cards.Commands.CreateCard;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Application.Tests.Cards.Commands.CreateCard
{
    public class CreateCardHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _context = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly Mock<IAppAuthorizationService> _authService = new();
        private readonly CreateCardHandler _handler;

        public CreateCardHandlerTests()
        {
            _handler = new CreateCardHandler(_context.Object, _currentUser.Object, _authService.Object);
            _currentUser
                .Setup(x => x.Id)
                .Returns(Guid.NewGuid());
        }

        [Fact]
        public async Task Handler_Should_ThrowNotFoundException_When_UserCannotAccessList()
        {
            // Arrange
            _authService
                .Setup(x => x.CanAccessListAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var cmd = new CreateCardCommand(Guid.NewGuid(), "Title", 1000);

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(cmd, CancellationToken.None))
                .Should()
                .ThrowAsync<NotFoundException>();

            _authService
                .Verify(x =>
                    x.CanAccessListAsync(
                        It.IsAny<EntityOperations>(),
                        It.IsAny<Guid>(),
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()), Times.Once);

            _context.Verify(x => x.Cards.AddAsync(It.IsAny<Card>(), It.IsAny<CancellationToken>()), Times.Never);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handler_Should_CreateList_When_UserCanAccessList()
        {
            // Arrange
            Guid listId = Guid.NewGuid();
            string title = "Title";
            decimal position = 1000m;

            _authService
                .Setup(x => x.CanAccessListAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            Card capturedCard = null!;
            _context
                .Setup(x => x.Cards.AddAsync(It.IsAny<Card>(), It.IsAny<CancellationToken>()))
                .Callback<Card, CancellationToken>((cl, _) => capturedCard = cl);

            var cmd = new CreateCardCommand(listId, title, position);

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            capturedCard.CardListId.Should().Be(listId);
            capturedCard.Title.Should().Be(title);
            capturedCard.Position.Should().Be(position);

            result.Id.Should().Be(capturedCard.Id);
            result.ListId.Should().Be(listId);
            result.Title.Should().Be(title);
            result.Position.Should().Be(position);

            _authService
                .Verify(x =>
                    x.CanAccessListAsync(
                        It.IsAny<EntityOperations>(),
                        It.IsAny<Guid>(),
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.Cards.AddAsync(It.IsAny<Card>(), It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
