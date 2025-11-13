using Application.Cards.DTOs;
using Application.Cards.Queries.GetCardById;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Domain.Entities;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;

namespace Application.Tests.Cards.Queries.GetCardById
{
    public class GetCardByIdHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _context = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly Mock<IAppAuthorizationService> _authService = new();
        private readonly GetCardByIdHandler _handler;
        public GetCardByIdHandlerTests()
        {
            _handler = new GetCardByIdHandler(_context.Object, _currentUser.Object, _authService.Object);
            _currentUser
                .Setup(x => x.Id)
                .Returns(Guid.NewGuid());
        }

        [Fact]
        public async Task Handler_Should_ThrowNotFoundException_When_UserCannotAccessCard()
        {
            // Arrange
            _authService
                .Setup(x => x.GetCardAsync(It.IsAny<Common.Enums.EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Card?)null);

            var query = new GetCardByIdQuery(Guid.NewGuid());

            // Act % Assert
            await FluentActions.Invoking(() => _handler.Handle(query, CancellationToken.None))
                .Should()
                .ThrowAsync<NotFoundException>();

            _authService.Verify(x =>
                x.GetCardAsync(
                    Common.Enums.EntityOperations.View,
                    query.CardId,
                    _currentUser.Object.Id,
                    CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Handler_Should_ReturnCardDto_When_UserCanAccessCard()
        {
            // Arrange
            var cardId = Guid.NewGuid();
            var userId = _currentUser.Object.Id;
            var card = new Card
            {
                Id = cardId,
                CardListId = Guid.NewGuid(),
                Title = "Test Card",
                Description = "Test Description",
                DueAt = DateTime.UtcNow.AddDays(1),
                Position = 1
            };

            _authService
                .Setup(x => x.GetCardAsync(Common.Enums.EntityOperations.View, cardId, userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(card);

            Guid firstUserId = Guid.NewGuid();
            Guid secondUserId = Guid.NewGuid();
            string firstUserName = "User1";
            string secondUserName = "User2";
            List<CardMember> cardMembers =
            [
                new () { CardId = cardId, UserId = firstUserId },
                new () { CardId = cardId, UserId = secondUserId }
            ];

            List<ApplicationUser> users =
            [
                new () { Id = firstUserId, UserName = firstUserName },
                new () { Id = secondUserId, UserName = secondUserName }
            ];
            _context
                .Setup(x => x.CardMembers)
                .ReturnsDbSet(cardMembers);

            _context
                .Setup(x => x.Users)
                .ReturnsDbSet(users);

            var query = new GetCardByIdQuery(cardId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(card.Id);
            result.ListId.Should().Be(card.CardListId);
            result.Title.Should().Be(card.Title);
            result.Description.Should().Be(card.Description);
            result.DueAt.Should().Be(card.DueAt);
            result.Position.Should().Be(card.Position);
            result.CardMembers.Should().HaveCount(2);

            var expectedMembers = new[]
            {
                new CardMemberDto(firstUserId, firstUserName),
                new CardMemberDto(secondUserId, secondUserName)
            };
            result.CardMembers.Should().BeEquivalentTo(expectedMembers, options => options.WithoutStrictOrdering());


            _authService.Verify(x =>
                x.GetCardAsync(
                    Common.Enums.EntityOperations.View,
                    cardId,
                    userId,
                    CancellationToken.None), Times.Once);
            _context.Verify(x => x.CardMembers, Times.Once);
            _context.Verify(x => x.Users, Times.Once);
        }
    }
}
