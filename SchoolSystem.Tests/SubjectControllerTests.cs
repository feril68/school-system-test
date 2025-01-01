using Microsoft.AspNetCore.Mvc;
using Moq;
using SchoolSystem.Controllers;
using SchoolSystem.Dto;
using SchoolSystem.Models;
using Xunit;

namespace SchoolSystem.Tests
{
    public class SubjectControllerTests
    {
        private readonly Mock<ISubjectService> _mockSubjectService;
        private readonly SubjectController _controller;

        public SubjectControllerTests()
        {
            _mockSubjectService = new Mock<ISubjectService>();
            _controller = new SubjectController(_mockSubjectService.Object);
        }

        [Fact]
        public async Task GetSubjects_ReturnsOkResult_WithListOfSubjects()
        {
            // Arrange
            var Subjects = new List<Subject>
            {
                new Subject { Id = 1, Name = "Math" },
                new Subject { Id = 2, Name = "Science" }
            };

            _mockSubjectService.Setup(s => s.GetAllSubjectsAsync()).ReturnsAsync(Subjects);

            // Act
            var result = await _controller.GetSubjects();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedSubjects = Assert.IsAssignableFrom<IEnumerable<Subject>>(okResult.Value);
            Assert.Equal(2, returnedSubjects.Count());
        }

        [Fact]
        public async Task GetSubject_ReturnsOkResult_WithSubject()
        {
            // Arrange
            var Subject = new Subject { Id = 1, Name = "Math" };
            _mockSubjectService.Setup(s => s.GetSubjectByIdAsync(1)).ReturnsAsync(Subject);

            // Act
            var result = await _controller.GetSubject(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedSubject = Assert.IsType<Subject>(okResult.Value);
            Assert.Equal("Math", returnedSubject.Name);
        }

        [Fact]
        public async Task CreateSubject_ReturnsCreatedAtActionResult_WithCreatedSubject()
        {
            // Arrange
            var SubjectDto = new SubjectCreateDto { Name = "Math" };
            var createdSubject = new Subject { Id = 1, Name = "Math" };

            _mockSubjectService.Setup(s => s.CreateSubjectAsync(SubjectDto)).ReturnsAsync(createdSubject);

            // Act
            var result = await _controller.CreateSubject(SubjectDto);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedSubject = Assert.IsType<Subject>(createdAtResult.Value);
            Assert.Equal("Math", returnedSubject.Name);
            Assert.Equal(1, returnedSubject.Id);
        }

        [Fact]
        public async Task UpdateSubject_ReturnsNoContent()
        {
            // Arrange
            var SubjectDto = new SubjectUpdateDto { Name = "Math Discrete" };

            _mockSubjectService.Setup(s => s.UpdateSubjectAsync(1, SubjectDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateSubject(1, SubjectDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteSubject_ReturnsNoContent()
        {
            // Arrange
            _mockSubjectService.Setup(s => s.DeleteSubjectAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteSubject(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
