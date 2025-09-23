using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Application.List.Queries.GetListById;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Application.Tests.Lists.Queries.GetListById
{
    public class GetListByIdHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _context = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly Mock<IAppAuthorizationService> _authService = new();
        private readonly GetListByIdHandler _handler;

        public GetListByIdHandlerTests()
        {
            _handler = new GetListByIdHandler(_context.Object, _currentUser.Object, _authService.Object);
            _currentUser
                .Setup(x => x.Id)
                .Returns(Guid.NewGuid());
        }

        [Fact]
        public async Task Handler_Should_ThrowNotFoundException_When_UserCannotAccessList()
        {
            // Arrange
            _authService
                .Setup(x => x.GetListAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((CardList?)null);

            var query = new GetListByIdQuery(Guid.NewGuid());

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(query, CancellationToken.None))
                .Should()
                .ThrowAsync<Exception>();
        }

        [Fact]
        public async Task Handler_Should_ReturnListDto_When_UserCanAccessList()
        {
            // Arrange
            Guid listId = Guid.NewGuid();
            Guid boardId = Guid.NewGuid();
            string title = "Test List";
            decimal position = 30000m;
            var cardList = new CardList
            {
                Id = listId,
                BoardId = boardId,
                Title = title,
                Position = position
            };

            _authService
                .Setup(x => x.GetListAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cardList);

            var query = new GetListByIdQuery(listId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(listId);
            result.BoardId.Should().Be(boardId);
            result.Title.Should().Be(title);
            result.Position.Should().Be(position);
        }
    }
}
