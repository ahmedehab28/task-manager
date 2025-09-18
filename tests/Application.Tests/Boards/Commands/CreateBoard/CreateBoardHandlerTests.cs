using Application.Boards.Commands.CreateBoard;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Moq;

namespace Application.Tests.Boards.Commands.CreateBoard
{
    public class CreateBoardHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _context = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly Mock<IAppAuthorizationService> _authService = new();
        private readonly CreateBoardHandler _handler;
        public CreateBoardHandlerTests()
        {
            _handler = new CreateBoardHandler(_context.Object, _currentUser.Object, _authService.Object);
            _currentUser
                .Setup(x => x.Id)
                .Returns(It.IsAny<Guid>());
        }

        [Fact]
        public async Task Handler_Should_CreateBoard_When_UserCanAccessProject()
        {
            // Arrange
            Guid projectId = Guid.NewGuid();

            string boardTitle = "Board Title";
            string boardDescription = "Board Description";

            _authService
                .Setup(x => x.CanAccessProject(It.IsAny<EntityOperations>(), projectId, It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            Board capturedBoard = null!;
            _context
                .Setup(x => x.Boards.AddAsync(It.IsAny<Board>(), It.IsAny<CancellationToken>()))
                .Callback<Board, CancellationToken>((b, _) => capturedBoard = b);

            _context
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var cmd = new CreateBoardCommand(projectId, boardTitle, boardDescription);

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            capturedBoard.Title.Should().Be(boardTitle);
            capturedBoard.Description.Should().Be(boardDescription);
            capturedBoard.BoardType.Should().Be(BoardType.Normal);

            result.Should().NotBeNull();
            result.Id.Should().Be(capturedBoard.Id);
            result.ProjectId.Should().Be(capturedBoard.ProjectId);
            result.Title.Should().Be(capturedBoard.Title);
            result.Description.Should().Be(capturedBoard.Description);

            _authService
                .Verify(x => x.CanAccessProject(It.IsAny<EntityOperations>(), projectId, It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.Boards.AddAsync(It.IsAny<Board>(), It.IsAny<CancellationToken>()), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handler_Should_ThrowNotFoundException_When_UserCannotAccessProject()
        {
            // Arrange
            Guid projectId = Guid.NewGuid();

            _authService
                .Setup(x => x.CanAccessProject(It.IsAny<EntityOperations>(), projectId, It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var cmd = new CreateBoardCommand(projectId, "Title", "Description");

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(cmd, CancellationToken.None))
                .Should()
                .ThrowAsync<NotFoundException>();

            _authService
                .Verify(x => x.CanAccessProject(It.IsAny<EntityOperations>(), projectId, It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            
            _context.Verify(x => x.Boards.AddAsync(It.IsAny<Board>(), It.IsAny<CancellationToken>()), Times.Never);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

    }
}
