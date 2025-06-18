using Domain.Entities.Boards;
using FluentAssertions;
using Infrastructure.Repositories.Boards;

namespace Infrastructure.Tests.Repositories.Boards.InMemoryBoardRepositoryTests
{
    public class AddTests
    {
        [Fact]
        public void AddBoard_WithValidBoard_ShouldAddNewBoardToBoardsList()
        {
            // Arrange
            Board board = new("Valid Title");
            InMemoryBoardRepository _repository = new();

            // Act
            _repository.Add(board);
            var result = _repository.GetById(board.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(board.Id);
            result.Title.Should().Be(board.Title);
            result.Description.Should().Be(board.Description);
            result.CreatedAt.Should().Be(board.CreatedAt);
        }
        [Fact]
        public void AddBoard_WithMoreThanOneValidBoard_ShouldAddNewBoardToBoardsList()
        {
            // Arrange
            Board board1 = new("Board1 Title");
            Board board2 = new("Board2 Title");
            Board board3 = new("Board3 Title");
            InMemoryBoardRepository _repository = new();

            // Act
            _repository.Add(board1);
            _repository.Add(board2);
            _repository.Add(board3);
            var allBoards = _repository.GetAll();

            // Assert
            allBoards.Count().Should().Be(3);
            allBoards.Should().Contain(b => b.Id == board1.Id && b.Title == board1.Title);
            allBoards.Should().Contain(b => b.Id == board2.Id && b.Title == board2.Title);
            allBoards.Should().Contain(b => b.Id == board3.Id && b.Title == board3.Title);
        }
        [Fact]
        public void Add_WithNullBoard_ShouldThrowArguementNullException()
        {
            // Arrange
            InMemoryBoardRepository _repository = new();

            // Act
            Action act = () =>
            {
                _repository.Add(null!);
            };

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("board");
        }
    }
}
