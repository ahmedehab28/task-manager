using Application.Cards.Commands.DeleteCard;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Application.Tests.Cards.Commands.DeleteCard
{
    public class DeleteCardHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _context = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly Mock<IAppAuthorizationService> _authService = new();
        private readonly DeleteCardHandler _handler;
        public DeleteCardHandlerTests()
        {
            _handler = new DeleteCardHandler(_context.Object, _currentUser.Object, _authService.Object);
            _currentUser
                .Setup(x => x.Id)
                .Returns(It.IsAny<Guid>());
        }
        [Fact]
        public async Task Handler_Should_ThrowNotFoundException_When_User_CannotAccessCard()
        {
            // Arrange
            _authService
                .Setup(x => x.GetCardAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Card?)null);

            var cmd = new DeleteCardCommand(Guid.NewGuid());

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(cmd, CancellationToken.None))
                .Should()
                .ThrowAsync<NotFoundException>();

            _authService
                .Verify(x =>
                    x.GetCardAsync(
                        It.IsAny<EntityOperations>(),
                        It.IsAny<Guid>(),
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()), Times.Once);

            _context.Verify(x => x.Cards.Remove(It.IsAny<Card>()), Times.Never);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handler_Should_DeleteCard_When_User_CanAcessCard()
        {
            // Arrange
            Guid cardId = Guid.NewGuid();

            Card fakeCard = new();

            _authService
                .Setup(x => x.GetCardAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeCard);

            Card deletedCard = null!;
            _context
                .Setup(x => x.Cards.Remove(It.IsAny<Card>()))
                .Callback<Card>(c => deletedCard = c);

            _context
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var cmd = new DeleteCardCommand(Guid.NewGuid());

            // Act
            await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            deletedCard.Should().BeSameAs(fakeCard);

            _authService
                .Verify(x =>
                    x.GetCardAsync(
                        It.IsAny<EntityOperations>(),
                        It.IsAny<Guid>(),
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.Cards.Remove(It.IsAny<Card>()), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
