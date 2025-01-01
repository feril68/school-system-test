using Microsoft.AspNetCore.Mvc;
using Moq;
using SchoolSystem.Controllers;
using SchoolSystem.Dto;
using SchoolSystem.Models;
using Xunit;

namespace SchoolSystem.Tests
{
    public class StudentControllerTests
    {
        private readonly Mock<IStudentService> _mockStudentService;
        private readonly StudentController _controller;

        public StudentControllerTests()
        {
            _mockStudentService = new Mock<IStudentService>();
            _controller = new StudentController(_mockStudentService.Object);
        }

        [Fact]
        public async Task GetStudents_ReturnsOkResult_WithListOfStudents()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Id = 1, Name = "John Doe", Email="john.doe@gmail.com", PhoneNumber="+6281234123123" },
                new Student { Id = 2, Name = "Jane Doe", Email="jane.doe@gmail.com", PhoneNumber="+6281234123122" }
            };

            _mockStudentService.Setup(s => s.GetAllStudentsAsync()).ReturnsAsync(students);

            // Act
            var result = await _controller.GetStudents();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedStudents = Assert.IsAssignableFrom<IEnumerable<Student>>(okResult.Value);
            Assert.Equal(2, returnedStudents.Count());
        }

        [Fact]
        public async Task GetStudent_ReturnsOkResult_WithStudent()
        {
            // Arrange
            var student = new Student { Id = 1, Name = "John Doe", Email = "john.doe@gmail.com", PhoneNumber = "+6281234123123" };
            _mockStudentService.Setup(s => s.GetStudentByIdAsync(1)).ReturnsAsync(student);

            // Act
            var result = await _controller.GetStudent(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedStudent = Assert.IsType<Student>(okResult.Value);
            Assert.Equal("John Doe", returnedStudent.Name);
        }

        [Fact]
        public async Task CreateStudent_ReturnsCreatedAtActionResult_WithCreatedStudent()
        {
            // Arrange
            var studentDto = new StudentCreateDto { Name = "John Doe", Email = "john.doe@gmail.com", PhoneNumber = "+6281234123123" };
            var createdStudent = new Student { Id = 1, Name = "John Doe", Email = "john.doe@gmail.com", PhoneNumber = "+6281234123123" };

            _mockStudentService.Setup(s => s.CreateStudentAsync(studentDto)).ReturnsAsync(createdStudent);

            // Act
            var result = await _controller.CreateStudent(studentDto);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedStudent = Assert.IsType<Student>(createdAtResult.Value);
            Assert.Equal("John Doe", returnedStudent.Name);
            Assert.Equal(1, returnedStudent.Id);
        }

        [Fact]
        public async Task UpdateStudent_ReturnsNoContent()
        {
            // Arrange
            var studentDto = new StudentUpdateDto { Name = "Updated Name", Email = "john.doe@gmail.com", PhoneNumber = "+6281234123123" };

            _mockStudentService.Setup(s => s.UpdateStudentAsync(1, studentDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateStudent(1, studentDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteStudent_ReturnsNoContent()
        {
            // Arrange
            _mockStudentService.Setup(s => s.DeleteStudentAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteStudent(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
