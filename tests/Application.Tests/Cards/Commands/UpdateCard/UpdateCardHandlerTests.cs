using Application.Cards.Commands.UpdateCard;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Application.Tests.Cards.Commands.UpdateCard
{
    public class UpdateCardHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _context = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly Mock<IAppAuthorizationService> _authService = new();
        private readonly UpdateCardHandler _handler;
        public UpdateCardHandlerTests()
        {
            _handler = new UpdateCardHandler(_context.Object, _currentUser.Object, _authService.Object);
            _currentUser
                .Setup(x => x.Id)
                .Returns(Guid.NewGuid());
        }

        [Fact]
        public async Task Handler_Should_ThrowNotFoundException_When_CardNotFound()
        {
            // Arrange
            _authService
                .Setup(x => x.GetCardAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Card?)null);

            var cmd = new UpdateCardCommand(Guid.NewGuid());

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(cmd, CancellationToken.None))
                .Should()
                .ThrowAsync<NotFoundException>();

            _authService
                .Verify(x => 
                    x.GetCardAsync(EntityOperations.Update, cmd.CardId, _currentUser.Object.Id, It.IsAny<CancellationToken>()), Times.Once);

            _authService
                .Verify(x => 
                    x.CanAccessListAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handler_Should_ThrowNotFoundException_When_TargetListNotFound()
        {
            // Arrange
            _authService
                .Setup(x => x.GetCardAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Card());

            _authService
                .Setup(x => x.CanAccessListAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var cmd = new UpdateCardCommand(Guid.NewGuid(), TargetListId: Guid.NewGuid());

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(cmd, CancellationToken.None))
                .Should()
                .ThrowAsync<NotFoundException>();

            _authService
                .Verify(x =>
                    x.GetCardAsync(
                        EntityOperations.Update, cmd.CardId, _currentUser.Object.Id, It.IsAny<CancellationToken>()), Times.Once);

            _authService
                .Verify(x =>
                    x.CanAccessListAsync(
                        It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task Handler_Should_MakeNoChanges_When_NoFieldsAreAssigned()
        {
            // Arrange
            Guid cardId = Guid.NewGuid();
            string oldTitle = "Old Title";
            string oldDescription = "Old Description";
            DateTime oldDueAt = DateTime.UtcNow.AddDays(3);
            decimal oldPosition = 1000m;
            Guid oldCardListId = Guid.NewGuid();

            var existingCard = new Card
            {
                Id = cardId,
                CardListId = oldCardListId,
                Title = oldTitle,
                Description = oldDescription,
                DueAt = oldDueAt,
                Position = oldPosition
            };

            _authService
                .Setup(x => x.GetCardAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCard);

            var cmd = new UpdateCardCommand(existingCard.Id);

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(existingCard.Id);
            result.ListId.Should().Be(existingCard.CardListId);
            result.Title.Should().Be(existingCard.Title);
            result.Description.Should().Be(existingCard.Description);
            result.DueAt.Should().Be(existingCard.DueAt);
            result.Position.Should().Be(existingCard.Position);

            _authService.Verify(x =>
                x.GetCardAsync(
                    EntityOperations.Update, cmd.CardId, _currentUser.Object.Id, It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            _authService.Verify(x =>
                x.CanAccessListAsync(
                    It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handler_Should_UpdateCard_When_FieldsAreAssigned()
        {
            // Arrange
            Guid cardId = Guid.NewGuid();

            string oldTitle = "Old Title";
            string oldDescription = "Old Description";
            DateTime oldDueAt = DateTime.UtcNow.AddDays(3);
            decimal oldPosition = 1000m;
            Guid oldCardListId = Guid.NewGuid();

            string newTitle = "New Title";
            string newDescription = "New Description";
            DateTime newDueAt = DateTime.UtcNow.AddDays(5);
            decimal newPosition = 2000m;
            Guid newCardListId = Guid.NewGuid();

            var existingCard = new Card
            {
                Id = cardId,
                CardListId = oldCardListId,
                Title = oldTitle,
                Description = oldDescription,
                DueAt = oldDueAt,
                Position = oldPosition
            };

            _authService
                .Setup(x => x.GetCardAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCard);
            _authService
                .Setup(x => x.CanAccessListAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var cmd = new UpdateCardCommand(
                existingCard.Id,
                Title: newTitle,
                Description: newDescription,
                DueAt: newDueAt,
                Position: newPosition,
                TargetListId: newCardListId);

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(existingCard.Id);
            result.ListId.Should().Be(newCardListId);
            result.Title.Should().Be(newTitle);
            result.Description.Should().Be(newDescription);
            result.DueAt.Should().Be(newDueAt);
            result.Position.Should().Be(newPosition);

            _authService.Verify(x =>
                x.GetCardAsync(
                    EntityOperations.Update, cmd.CardId, _currentUser.Object.Id, It.IsAny<CancellationToken>()), Times.Once);
            _authService.Verify(x =>
                x.CanAccessListAsync(
                    EntityOperations.AddToParent, newCardListId, _currentUser.Object.Id, It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
