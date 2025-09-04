//using Application.Boards.Commands.CreateBoard;
//using Application.Common.Interfaces;
//using Domain.Entities;
//using FluentAssertions;
//using Moq;

//namespace Application.Tests.Boards.Commands.CreateBoard
//{
//    public class CreateBoardHandlerTests
//    {
//        private readonly Mock<IBoardRepository> _mockRepo;
//        private readonly CreateBoardHandler _handler;
//        public CreateBoardHandlerTests()
//        {
//            _mockRepo = new Mock<IBoardRepository>();
//            _handler = new CreateBoardHandler(_mockRepo.Object);
            
//        }

//        [Fact]
//        public async Task CreateBoardHandler_WithValidInput_ShouldCreateBoard()
//        {
//            // Arrange
//            string boardTitle = "Board Title";
//            string boardDescription = "Board Description";
//            var cmd = new CreateBoardCommand(boardTitle, boardDescription);
//            Board saved = null!;
//            var setup = _mockRepo
//                .Setup(r => r.AddAsync(It.IsAny<Board>()))
//                .Callback<Board>(board => saved = board);

//            // Act
//            var id = await _handler.Handle(cmd, CancellationToken.None);

//            // Assertion
//            id.Should().NotBe(Guid.Empty);
//            saved.Should().NotBeNull();
//            saved.Id.Should().Be(id);
//            saved.Title.Should().Be(boardTitle);
//            saved.Description.Should().Be(boardDescription);

//        }

//    }
//}
