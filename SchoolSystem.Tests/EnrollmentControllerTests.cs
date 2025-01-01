using Microsoft.AspNetCore.Mvc;
using Moq;
using SchoolSystem.Controllers;
using SchoolSystem.Dto;
using SchoolSystem.Models;
using SchoolSystem.Services;
using Xunit;

namespace SchoolSystem.Tests
{
    public class EnrollmentControllerTests
    {
        private readonly Mock<IEnrollmentService> _mockService;
        private readonly EnrollmentController _controller;

        public EnrollmentControllerTests()
        {
            _mockService = new Mock<IEnrollmentService>();
            _controller = new EnrollmentController(_mockService.Object);
        }

        [Fact]
        public async Task GetEnrollmentsByStudent_ReturnsOkResultWithEnrollments()
        {
            // Arrange
            var studentId = 1;
            var classId = 1;
            var student = new Student { Id = studentId, Name = "John Doe" };
            var classObj = new Class { Id = classId, Name = "class one" };

            var enrollments = new List<Enrollment>
            {
                new Enrollment { Id = 1, StudentId = studentId, Student = student, Class = classObj, ClassId = 1, EnrollmentDate = System.DateTime.UtcNow },
            };
            _mockService.Setup(s => s.GetEnrollmentsByStudentAsync(studentId)).ReturnsAsync(enrollments);

            // Act
            var result = await _controller.GetEnrollmentsByStudent(studentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedEnrollments = Assert.IsType<List<Enrollment>>(okResult.Value);
            Assert.Equal(enrollments.Count, returnedEnrollments.Count);
        }

        [Fact]
        public async Task CreateEnrollment_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var enrollmentDto = new EnrollmentCreateDto { ClassId = 1, StudentId = 1, EnrollmentDate = System.DateTime.UtcNow };
            var student = new Student { Id = enrollmentDto.StudentId, Name = "John Doe" };
            var classObj = new Class { Id = enrollmentDto.ClassId, Name = "class one" };
            var createdEnrollment = new Enrollment
            {
                Id = 1,
                ClassId = enrollmentDto.ClassId,
                Class = classObj,
                StudentId = enrollmentDto.StudentId,
                Student = student,
                EnrollmentDate = enrollmentDto.EnrollmentDate
            };

            _mockService.Setup(s => s.CreateEnrollmentAsync(enrollmentDto)).ReturnsAsync(createdEnrollment);

            // Act
            var result = await _controller.CreateEnrollment(enrollmentDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result); // Explicitly check Result
            var returnedEnrollment = Assert.IsType<Enrollment>(createdAtActionResult.Value);

            Assert.Equal(createdEnrollment.Id, returnedEnrollment.Id);
            Assert.Equal(createdEnrollment.ClassId, returnedEnrollment.ClassId);
            Assert.Equal(createdEnrollment.StudentId, returnedEnrollment.StudentId);
        }


        [Fact]
        public async Task DeleteEnrollment_ReturnsNoContentResult_WhenEnrollmentExists()
        {
            // Arrange
            var enrollmentId = 1;
            _mockService.Setup(s => s.DeleteEnrollmentAsync(enrollmentId));

            // Act
            var result = await _controller.DeleteEnrollment(enrollmentId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
