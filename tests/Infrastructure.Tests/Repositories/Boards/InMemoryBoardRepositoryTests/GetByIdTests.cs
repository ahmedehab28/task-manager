using Domain.Entities.Boards;
using FluentAssertions;
using Infrastructure.Repositories.Boards;

namespace Infrastructure.Tests.Repositories.Boards.InMemoryBoardRepositoryTests
{
    public class GetByIdTests
    {
        [Fact]
        public void GetById_WithValidIdAndRepoWithOneBoard_ShouldReturnBoard()
        {
            // Arrange
            Board board = new("Board Title");
            InMemoryBoardRepository _repository = new();
            _repository.Add(board);

            // Act
            Board? result = _repository.GetById(board.Id);

            // Assert
            result!.Id.Should().Be(result.Id);
            result!.Title.Should().Be(board.Title);
            result!.Description.Should().Be(board.Description);
            result!.CreatedAt.Should().Be(board.CreatedAt);
        }

        [Fact]
        public void GetById_WithValidIdAndRepoWithMoreThanOneBoard_ShouldReturnBoard()
        {
            // Arrange
            Board board1 = new("Board1 Title");
            Board board2 = new("Board2 Title");
            Board board3 = new("Board3 Title");

            InMemoryBoardRepository _repository = new();
            _repository.Add(board1);
            _repository.Add(board2);
            _repository.Add(board3);

            // Act
            Board? result1 = _repository.GetById(board1.Id);
            Board? result2 = _repository.GetById(board2.Id);
            Board? result3 = _repository.GetById(board3.Id);

            // Assertion
            result1!.Title.Should().Be(board1.Title);
            result2!.Title.Should().Be(board2.Title);
            result3!.Title.Should().Be(board3.Title);
        }
    }
}
