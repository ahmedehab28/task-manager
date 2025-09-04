//using Domain.Entities;
//using FluentAssertions;
//using Infrastructure.Repositories.Boards;

//namespace Infrastructure.Tests.Repositories.Boards.InMemoryBoardRepositoryTests
//{
//    public class AddTests
//    {
//        [Fact]
//        public async Task AddBoard_WithValidBoard_ShouldAddNewBoardToBoardsList()
//        {
//            // Arrange
//            Board board = new("Valid Title");
//            InMemoryBoardRepository _repository = new();

//            // Act
//            await _repository.AddAsync(board);
//            var result = await _repository.GetByIdAsync(board.Id);

//            // Assert
//            result.Should().NotBeNull();
//            result.Id.Should().Be(board.Id);
//            result.Title.Should().Be(board.Title);
//            result.Description.Should().Be(board.Description);
//            result.CreatedAt.Should().Be(board.CreatedAt);
//        }
//        [Fact]
//        public async Task AddBoard_WithMoreThanOneValidBoard_ShouldAddNewBoardToBoardsList()
//        {
//            // Arrange
//            Board board1 = new("Board1 Title");
//            Board board2 = new("Board2 Title");
//            Board board3 = new("Board3 Title");
//            InMemoryBoardRepository _repository = new();

//            // Act
//            await _repository.AddAsync(board1);
//            await _repository.AddAsync(board2);
//            await _repository.AddAsync(board3);
//            var allBoards = await _repository.GetAllAsync();

//            // Assert
//            allBoards.Count().Should().Be(3);
//            allBoards.Should().Contain(b => b.Id == board1.Id && b.Title == board1.Title);
//            allBoards.Should().Contain(b => b.Id == board2.Id && b.Title == board2.Title);
//            allBoards.Should().Contain(b => b.Id == board3.Id && b.Title == board3.Title);
//        }
//        [Fact]
//        public async Task Add_WithNullBoard_ShouldThrowArguementNullException()
//        {
//            // Arrange
//            InMemoryBoardRepository _repository = new();

//            // Act
//            Func<Task> act = () =>_repository.AddAsync(null!);

//            // Assert
//            await act.Should()
//                .ThrowAsync<ArgumentNullException>()
//                .WithParameterName("board");
//        }
//    }
//}
