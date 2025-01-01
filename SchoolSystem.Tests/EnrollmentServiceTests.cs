using Microsoft.EntityFrameworkCore;
using SchoolSystem.Data;
using SchoolSystem.Dto;
using SchoolSystem.Exceptions;
using SchoolSystem.Models;
using SchoolSystem.Services;
using Xunit;

namespace SchoolSystem.Tests
{
    public class EnrollmentServiceTests
    {
        private readonly DbContextOptions<AppDbContext> _options;
        private readonly AppDbContext _context;
        private readonly EnrollmentService _service;

        public EnrollmentServiceTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(_options);
            _service = new EnrollmentService(_context);

            // Seed data
            var subject = new Subject { Id = 1, Name = "Math" };
            var teacher = new Teacher { Id = 1, Name = "Mr. Smith" };
            var student = new Student { Id = 1, Name = "John Doe" };

            _context.Subjects.Add(subject);
            _context.Teachers.Add(teacher);
            _context.Students.Add(student);

            var classObj = new Class { Id = 1, Name = "Class A", Subject = subject, Teacher = teacher };
            _context.Classes.Add(classObj);

            var enrollment = new Enrollment { Id = 1, Student = student, Class = classObj, EnrollmentDate = DateTime.UtcNow };
            _context.Enrollments.Add(enrollment);

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetEnrollmentsByStudentAsync_ReturnsEnrollmentsForStudent()
        {
            // Act
            var result = await _service.GetEnrollmentsByStudentAsync(1);

            // Assert
            Assert.Single(result);
            Assert.Equal("Class A", result.First().Class.Name);
            Assert.Equal("Math", result.First().Class.Subject!.Name);
            Assert.Equal("Mr. Smith", result.First().Class.Teacher!.Name);
        }

        [Fact]
        public async Task GetEnrollmentsByStudentAsync_ReturnsEmpty_WhenNoEnrollmentsExistForStudent()
        {
            // Act
            var result = await _service.GetEnrollmentsByStudentAsync(999);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task CreateEnrollmentAsync_AddsEnrollmentToDatabase()
        {
            // Arrange
            var newStudent = new Student { Id = 2, Name = "Jane Doe" };
            _context.Students.Add(newStudent);
            await _context.SaveChangesAsync();

            var enrollmentDto = new EnrollmentCreateDto
            {
                ClassId = 1, // Use existing class
                StudentId = 2, // New student
                EnrollmentDate = DateTime.UtcNow
            };

            // Act
            var result = await _service.CreateEnrollmentAsync(enrollmentDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newStudent.Id, result.Student.Id);
            Assert.Equal(1, result.Class.Id);
            Assert.Equal(2, _context.Enrollments.Count());
        }


        [Fact]
        public async Task CreateEnrollmentAsync_ThrowsException_WhenClassDoesNotExist()
        {
            // Arrange
            var enrollmentDto = new EnrollmentCreateDto
            {
                ClassId = 999,
                StudentId = 1,
                EnrollmentDate = DateTime.UtcNow
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _service.CreateEnrollmentAsync(enrollmentDto));

            Assert.Equal("Class with ID 999 does not exist", exception.Message);
        }

        [Fact]
        public async Task CreateEnrollmentAsync_ThrowsException_WhenStudentDoesNotExist()
        {
            // Arrange
            var enrollmentDto = new EnrollmentCreateDto
            {
                ClassId = 1,
                StudentId = 999,
                EnrollmentDate = DateTime.UtcNow
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _service.CreateEnrollmentAsync(enrollmentDto));

            Assert.Equal("Student with ID 999 does not exist", exception.Message);
        }

        [Fact]
        public async Task DeleteEnrollmentAsync_RemovesEnrollmentFromDatabase()
        {
            // Act
            await _service.DeleteEnrollmentAsync(1);

            // Assert
            Assert.Null(await _context.Enrollments.FindAsync(1));
        }

        [Fact]
        public async Task DeleteEnrollmentAsync_ReturnsFalse_WhenEnrollmentDoesNotExist()
        {
            // Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _service.DeleteEnrollmentAsync(999));
        }
    }
}
