using Microsoft.AspNetCore.Mvc;
using Moq;
using SchoolSystem.Controllers;
using SchoolSystem.Dto;
using SchoolSystem.Models;
using Xunit;

namespace SchoolSystem.Tests
{
    public class ClassControllerTests
    {
        private readonly Mock<IClassService> _mockClassService;
        private readonly ClassController _controller;

        public ClassControllerTests()
        {
            _mockClassService = new Mock<IClassService>();
            _controller = new ClassController(_mockClassService.Object);
        }

        [Fact]
        public async Task GetClasses_ReturnsOkResult_WithListOfClasss()
        {
            // Arrange
            var Classs = new List<Class>
            {
                new Class { Id = 1, Name = "class one" },
                new Class { Id = 2, Name = "class two" }
            };

            _mockClassService.Setup(s => s.GetAllClassesAsync()).ReturnsAsync(Classs);

            // Act
            var result = await _controller.GetClasses();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedClasses = Assert.IsAssignableFrom<IEnumerable<Class>>(okResult.Value);
            Assert.Equal(2, returnedClasses.Count());
        }

        [Fact]
        public async Task GetClass_ReturnsOkResult_WithClass()
        {
            // Arrange
            var Class = new Class { Id = 1, Name = "class one" };
            _mockClassService.Setup(s => s.GetClassByIdAsync(1)).ReturnsAsync(Class);

            // Act
            var result = await _controller.GetClass(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedClass = Assert.IsType<Class>(okResult.Value);
            Assert.Equal("class one", returnedClass.Name);
        }

        [Fact]
        public async Task CreateClass_ReturnsCreatedAtActionResult_WithCreatedClass()
        {
            // Arrange
            var ClassDto = new ClassCreateDto { Name = "class one" };
            var createdClass = new Class { Id = 1, Name = "class one" };

            _mockClassService.Setup(s => s.CreateClassAsync(ClassDto)).ReturnsAsync(createdClass);

            // Act
            var result = await _controller.CreateClass(ClassDto);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedClass = Assert.IsType<Class>(createdAtResult.Value);
            Assert.Equal("class one", returnedClass.Name);
            Assert.Equal(1, returnedClass.Id);
        }

        [Fact]
        public async Task UpdateClass_ReturnsNoContent()
        {
            // Arrange
            var ClassDto = new ClassUpdateDto { Name = "Updated class one" };

            _mockClassService.Setup(s => s.UpdateClassAsync(1, ClassDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateClass(1, ClassDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteClass_ReturnsNoContent()
        {
            // Arrange
            _mockClassService.Setup(s => s.DeleteClassAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteClass(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
