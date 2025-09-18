using Application.Boards.DTOs;
using Application.Boards.Queries.GetAllBoards;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace Application.Tests.Boards.Queries.GetAllBoards
{
    public class GetAllBoardsHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _context = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly Mock<IAppAuthorizationService> _authService = new();
        private readonly GetAllBoardsHandler _handler;

        public GetAllBoardsHandlerTests()
        {
            _handler = new GetAllBoardsHandler(_context.Object, _currentUser.Object, _authService.Object);
            _currentUser
                .Setup(x => x.Id)
                .Returns(It.IsAny<Guid>());
        }

        [Fact]
        public async Task Handler_Should_ThrowNotFoundException_When_UserCannotAccessProject()
        {
            // Arrange
            _authService
                .Setup(x => x.CanAccessProject(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var cmd = new GetAllBoardsQuery(Guid.NewGuid());

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(cmd, CancellationToken.None))
                .Should()
                .ThrowAsync<NotFoundException>();

            _authService.Verify(x =>
                x.CanAccessProject(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _context.Verify(x => x.Boards, Times.Never);
        }

        [Fact]
        public async Task Handler_Should_ReturnEmptyList_When_ProjectHasNoBoards()
        {
            // Arrange
            Guid projectId = Guid.NewGuid();

            Guid boardOneId = Guid.NewGuid();
            string boardOneTitle = "B1 Title";
            string boardOneDescription = "B2 Description";

            Guid boardTwoId = Guid.NewGuid();
            string boardTwoTitle = "B2 Title";
            string boardTwoDescription = "B2 Description";

            List<Board> boards = [
                new () { Id = boardOneId, ProjectId = Guid.NewGuid(), Title = boardOneTitle, Description = boardOneDescription },
                new () { Id = boardTwoId, ProjectId = Guid.NewGuid(), Title = boardTwoTitle, Description = boardTwoDescription },
            ];

            var mockBoards = boards.BuildMockDbSet();

            _authService
                .Setup(x => x.CanAccessProject(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _context
                .Setup(x => x.Boards)
                .Returns(mockBoards.Object);

            var cmd = new GetAllBoardsQuery(projectId);

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            result.Should().HaveCount(0);

        }

        [Fact]
        public async Task Handler_Should_ReturnBoardsList_When_ProjectHasBoards()
        {
            // Arrange
            Guid projectId = Guid.NewGuid();

            Guid boardOneId = Guid.NewGuid();
            string boardOneTitle = "B1 Title";
            string boardOneDescription = "B2 Description";
            DateTime boardOneCreatedAt = DateTime.UtcNow.AddMinutes(50);

            Guid boardTwoId = Guid.NewGuid();
            string boardTwoTitle = "B2 Title";
            string boardTwoDescription = "B2 Description";
            DateTime boardTwoCreatedAt = DateTime.UtcNow.AddMinutes(100);


            Guid boardThreeId = Guid.NewGuid();
            string boardThreeTitle = "B3 Title";
            string boardThreeDescription = "B3 Description";
            DateTime boardThreeCreatedAt = DateTime.UtcNow.AddMinutes(-100);


            Guid boardFourId = Guid.NewGuid();
            string boardFourTitle = "B4 Title";
            string boardFourDescription = "B4 Description";
            DateTime boardFourCreatedAt = DateTime.UtcNow;

            List<Board> boards = [
                new () { Id = boardOneId, ProjectId = projectId, Title = boardOneTitle, Description = boardOneDescription, CreatedAt = boardOneCreatedAt },
                new () { Id = boardTwoId, ProjectId = projectId, Title = boardTwoTitle, Description = boardTwoDescription, CreatedAt = boardTwoCreatedAt },
                new () { Id = boardThreeId, ProjectId = projectId, Title = boardThreeTitle, Description = boardThreeDescription, CreatedAt = boardThreeCreatedAt },
                new () { Id = boardFourId, ProjectId = Guid.NewGuid(), Title = boardFourTitle, Description = boardFourDescription, CreatedAt = boardFourCreatedAt },
            ];

            var mockBoards = boards.BuildMockDbSet();

            _authService
                .Setup(x => x.CanAccessProject(It.IsAny<EntityOperations>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _context
                .Setup(x => x.Boards)
                .Returns(mockBoards.Object);

            var cmd = new GetAllBoardsQuery(projectId);

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            result.Should().HaveCount(3);
            result.Should().OnlyContain(b => b.ProjectId == projectId);
            result.Select(r => r.Id).Should().ContainInOrder(boardThreeId, boardOneId, boardTwoId);
            result.Should()
                .ContainEquivalentOf(new BoardDetailsDto(boardOneId, projectId, boardOneTitle, boardOneDescription));
        }
    }
}
