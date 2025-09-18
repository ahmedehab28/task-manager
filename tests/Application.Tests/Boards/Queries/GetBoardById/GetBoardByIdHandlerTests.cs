using Application.Boards.Queries.GetBoardById;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace Application.Tests.Boards.Queries.GetBoardById
{
    public class GetBoardByIdHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _context = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly Mock<IAppAuthorizationService> _authService = new();
        private readonly GetBoardByIdHandler _handler;

        public GetBoardByIdHandlerTests()
        {
            _handler = new GetBoardByIdHandler(_context.Object, _currentUser.Object, _authService.Object);
            _currentUser
                .Setup(x => x.Id)
                .Returns(It.IsAny<Guid>());
        }

        [Fact]
        public async Task Handler_Should_ThrowNotFoundException_When_UserCannotAccessProject()
        {
            // Arrange
            _authService
                .Setup(x => x.CanAccessBoardAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var query = new GetBoardByIdQuery(Guid.NewGuid());

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(query, CancellationToken.None))
                .Should()
                .ThrowAsync<NotFoundException>();

            _authService.Verify(x => 
                x.CanAccessBoardAsync(
                    It.IsAny<EntityOperations>(),
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()), Times.Once);

            _context.Verify(x => x.Boards.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()), Times.Never);
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

            var query = new GetBoardByIdQuery(Guid.NewGuid());

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(query, CancellationToken.None))
                .Should()
                .ThrowAsync<NotFoundException>();

            _authService.Verify(x =>
                x.CanAccessBoardAsync(
                    It.IsAny<EntityOperations>(),
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.Boards.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handler_Should_ReturnBoard_When_UserCanACcessBoard()
        {
            // Arrange
            Guid projectId = Guid.NewGuid();

            Guid boardOneId = Guid.NewGuid();
            string boardOneTitle = "B1 Title";
            string boardOneDescription = "B2 Description";

            Board board = new()
            {
                Id = boardOneId,
                Title = boardOneTitle,
                Description = boardOneDescription
            };

            _authService
                .Setup(x => x.CanAccessBoardAsync(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _context
                .Setup(x => x.Boards.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(board);


            var query = new GetBoardByIdQuery(boardOneId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Id.Should().Be(boardOneId);
            result.Title.Should().Be(boardOneTitle);
            result.Description.Should().Be(boardOneDescription);

            _authService.Verify(x =>
                x.CanAccessBoardAsync(
                    It.IsAny<EntityOperations>(),
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.Boards.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
