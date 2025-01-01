using Microsoft.AspNetCore.Mvc;
using Moq;
using SchoolSystem.Controllers;
using SchoolSystem.Dto;
using SchoolSystem.Models;
using Xunit;

namespace SchoolSystem.Tests
{
    public class TeacherControllerTests
    {
        private readonly Mock<ITeacherService> _mockTeacherService;
        private readonly TeacherController _controller;

        public TeacherControllerTests()
        {
            _mockTeacherService = new Mock<ITeacherService>();
            _controller = new TeacherController(_mockTeacherService.Object);
        }

        [Fact]
        public async Task GetTeachers_ReturnsOkResult_WithListOfTeachers()
        {
            // Arrange
            var Teachers = new List<Teacher>
            {
                new Teacher { Id = 1, Name = "John Doe", Email="john.doe@gmail.com", PhoneNumber="+6281234123123" },
                new Teacher { Id = 2, Name = "Jane Doe", Email="jane.doe@gmail.com", PhoneNumber="+6281234123124" }
            };

            _mockTeacherService.Setup(s => s.GetAllTeachersAsync()).ReturnsAsync(Teachers);

            // Act
            var result = await _controller.GetTeachers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTeachers = Assert.IsAssignableFrom<IEnumerable<Teacher>>(okResult.Value);
            Assert.Equal(2, returnedTeachers.Count());
        }

        [Fact]
        public async Task GetTeacher_ReturnsOkResult_WithTeacher()
        {
            // Arrange
            var Teacher = new Teacher { Id = 1, Name = "John Doe", Email = "john.doe@gmail.com", PhoneNumber = "+6281234123123" };
            _mockTeacherService.Setup(s => s.GetTeacherByIdAsync(1)).ReturnsAsync(Teacher);

            // Act
            var result = await _controller.GetTeacher(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTeacher = Assert.IsType<Teacher>(okResult.Value);
            Assert.Equal("John Doe", returnedTeacher.Name);
        }

        [Fact]
        public async Task CreateTeacher_ReturnsCreatedAtActionResult_WithCreatedTeacher()
        {
            // Arrange
            var TeacherDto = new TeacherCreateDto { Name = "John Doe", Email = "john.doe@gmail.com", PhoneNumber = "+6281234123123" };
            var createdTeacher = new Teacher { Id = 1, Name = "John Doe", Email = "john.doe@gmail.com", PhoneNumber = "+6281234123123" };

            _mockTeacherService.Setup(s => s.CreateTeacherAsync(TeacherDto)).ReturnsAsync(createdTeacher);

            // Act
            var result = await _controller.CreateTeacher(TeacherDto);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedTeacher = Assert.IsType<Teacher>(createdAtResult.Value);
            Assert.Equal("John Doe", returnedTeacher.Name);
            Assert.Equal(1, returnedTeacher.Id);
        }

        [Fact]
        public async Task UpdateTeacher_ReturnsNoContent()
        {
            // Arrange
            var TeacherDto = new TeacherUpdateDto { Name = "Updated Name", Email = "john.doe@gmail.com", PhoneNumber = "+6281234123123" };

            _mockTeacherService.Setup(s => s.UpdateTeacherAsync(1, TeacherDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateTeacher(1, TeacherDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTeacher_ReturnsNoContent()
        {
            // Arrange
            _mockTeacherService.Setup(s => s.DeleteTeacherAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteTeacher(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
