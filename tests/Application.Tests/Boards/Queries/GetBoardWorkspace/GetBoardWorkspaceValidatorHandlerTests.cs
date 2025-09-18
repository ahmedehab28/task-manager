using Application.Boards.DTOs;
using Application.Cards.Queries.GetBoardWorksoace;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace Application.Tests.Boards.Queries.GetBoardWorkspace
{
    public class GetBoardWorkspaceValidatorHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _context = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly Mock<IAppAuthorizationService> _authService = new();
        private readonly GetBoardWorkspaceHandler _handler;

        public GetBoardWorkspaceValidatorHandlerTests()
        {
            _handler = new GetBoardWorkspaceHandler(_context.Object, _currentUser.Object, _authService.Object);
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

            var query = new GetBoardWorkspaceQuery(Guid.NewGuid());

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(query, CancellationToken.None))
                .Should()
                .ThrowAsync<NotFoundException>();

            _authService
                .Verify(x =>
                    x.CanAccessBoardAsync(
                        It.IsAny<EntityOperations>(),
                        It.IsAny<Guid>(),
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()), Times.Once);

            _context.Verify(x => x.CardLists, Times.Never);
        }

        [Fact]
        public async Task Handler_Should_ReturnEmptyCardList_When_BoardHasNoCardLists()
        {
            // Arrange
            Guid boardOneId = Guid.NewGuid();
            Guid boardTwoId = Guid.NewGuid();
            Guid cardListId = Guid.NewGuid();

            List<CardList> lists =
            [
                new () { Id = cardListId, BoardId = boardTwoId, Title = "Title", Position = 1000 },
            ];

            List<Card> cards =
            [
                new () { CardListId = cardListId, Title = "Card", Position = 1200 }
            ];

            var mockLists = lists.BuildMockDbSet();
            var mockCards = cards.BuildMockDbSet();
            
            _authService
                .Setup(x => x.CanAccessBoardAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _context
                .Setup(x => x.CardLists)
                .Returns(mockLists.Object);

            _context
                .Setup(x => x.Cards)
                .Returns(mockCards.Object);

            var query = new GetBoardWorkspaceQuery(boardOneId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Id.Should().Be(boardOneId);
            result.Lists.Should().BeEmpty();
        }

        [Fact]
        public async Task Handler_Should_ReturnCardList_WithEmptyCards_When_BoardHasCardLists_ButNoCards()
        {
            // Arrange
            Guid boardOneId = Guid.NewGuid();
            Guid cardListOneId = Guid.NewGuid();
            Guid cardListTwoId = Guid.NewGuid();

            List<CardList> lists =
            [
                new () { Id = cardListOneId, BoardId = boardOneId, Title = "L1", Position = 100000 },
                new () { Id = cardListTwoId, BoardId = boardOneId, Title = "L2", Position = 1200 },
            ];

            List<Card> cards = [];

            var mockLists = lists.BuildMockDbSet();
            var mockCards = cards.BuildMockDbSet();

            _authService
                .Setup(x => x.CanAccessBoardAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _context
                .Setup(x => x.CardLists)
                .Returns(mockLists.Object);

            _context
                .Setup(x => x.Cards)
                .Returns(mockCards.Object);

            var query = new GetBoardWorkspaceQuery(boardOneId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Id.Should().Be(boardOneId);
            result.Lists.Should().HaveCount(2);
            result.Lists.Select(cl => cl.Position).Should().BeInAscendingOrder();
            result.Lists.Should().OnlyContain(cl => !cl.Cards.Any());
        }

        [Fact]
        public async Task Handler_Should_ReturnCardLists_When_BoardHasCardLists()
        {
            // Arrange
            Guid boardOneId = Guid.NewGuid();
            Guid boardTwoId = Guid.NewGuid();
            Guid cardListOneId = Guid.NewGuid();
            Guid cardListTwoId = Guid.NewGuid();
            Guid cardListThreeId = Guid.NewGuid();

            List<CardList> lists =
            [
                new () { Id = cardListOneId, BoardId = boardOneId, Title = "L1", Position = 1000 },
                new () { Id = cardListTwoId, BoardId = boardOneId, Title = "L2", Position = 800 },
                new () { Id = cardListThreeId, BoardId = boardTwoId, Title = "L3", Position = 9000 },
            ];

            List<Card> cards =
            [
                new () { CardListId = cardListOneId, Title = "Card1", Position = 1200 },
                new () { CardListId = cardListOneId, Title = "Card2", Position = 1100 },
                new () { CardListId = cardListThreeId, Title = "Card3", Position = 500 },
            ];

            var mockLists = lists.BuildMockDbSet();
            var mockCards = cards.BuildMockDbSet();

            _authService
                .Setup(x => x.CanAccessBoardAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _context
                .Setup(x => x.CardLists)
                .Returns(mockLists.Object);

            _context
                .Setup(x => x.Cards)
                .Returns(mockCards.Object);

            var query = new GetBoardWorkspaceQuery(boardOneId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Id.Should().Be(boardOneId);
            result.Lists.Should().HaveCount(2);
            result.Lists.Select(l => l.Id).Should().BeEquivalentTo(new[] { cardListOneId, cardListTwoId });
            result.Lists.Should().NotContain(cl => cl.Id == cardListThreeId);

            var expectedLists = lists
                .Where(l => l.BoardId == boardOneId)
                .OrderBy(l => l.Position)
                .Select(l => new BoardListsDto
                (
                    Id: l.Id,
                    BoardId: l.BoardId,
                    Title: l.Title,
                    Position: l.Position,
                    Cards: cards
                        .Where(c => c.CardListId == l.Id)
                        .OrderBy(c => c.Position)
                        .Select(c => new BoardCardsDto
                        (
                            Id: c.Id,
                            BoardId: l.BoardId,
                            ListId: c.CardListId,
                            Title: c.Title,
                            Position: c.Position,
                            Description: c.Description,
                            DueAt: c.DueAt
                        )).ToList()
                )).ToList();

            var expectedResult = new BoardDto(Id: boardOneId, Lists: expectedLists);
        }
    }
}
