
//using Application.Boards.Commands.CreateBoard;
//using FluentValidation.TestHelper;

//namespace Application.Tests.Boards.Commands.CreateBoard
//{
//    public class CreateBoardValidatorTests
//    {
//        private readonly CreateBoardValidator _validator;

//        public CreateBoardValidatorTests()
//        {
//            _validator = new CreateBoardValidator();
//        }

//        [Theory]
//        [InlineData(null)]
//        [InlineData("")]
//        [InlineData(" ")]
//        [InlineData("    ")]
//        public void CreateBoardValidtaor_WithInvalidInput_ShouldFailValidation(string? title)
//        {
//            // Arrange
//            var cmd = new CreateBoardCommand(title!, "Board Description");

//            // Act
//            var result = _validator.TestValidate(cmd);

//            // Assertion
//            result.ShouldHaveValidationErrorFor(x => x.Title);

//        }

//        [Theory]
//        [InlineData("1")]
//        [InlineData("T")]
//        [InlineData("#")]
//        [InlineData("T1#")]
//        [InlineData("Board Title")]
//        public void CreateBoardValidator_WithValidInput_ShouldPassValidation(string title)
//        {
//            // Arrange
//            var cmd = new CreateBoardCommand(title, "Description");

//            // Act
//            var result = _validator.TestValidate(cmd);

//            // Assertion
//            result.ShouldNotHaveValidationErrorFor(x => x.Title);
//        }

        
//    }
//}
