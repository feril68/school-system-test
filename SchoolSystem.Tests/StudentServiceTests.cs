using Microsoft.EntityFrameworkCore;
using SchoolSystem.Data;
using SchoolSystem.Dto;
using SchoolSystem.Exceptions;
using SchoolSystem.Models;
using SchoolSystem.Services;
using Xunit;

namespace SchoolSystem.Tests
{
    public class StudentServiceTests
    {
        private readonly DbContextOptions<AppDbContext> _options;
        private readonly AppDbContext _context;
        private readonly StudentService _service;

        public StudentServiceTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(_options);
            _service = new StudentService(_context);

            // Seed data
            _context.Students.AddRange(
                new Student { Id = 1, Name = "John Doe" },
                new Student { Id = 2, Name = "Jane Doe" }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllStudentsAsync_ReturnsAllStudents()
        {
            // Act
            var result = await _service.GetAllStudentsAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetStudentByIdAsync_ReturnsStudent_WhenStudentExists()
        {
            // Act
            var result = await _service.GetStudentByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
        }

        [Fact]
        public async Task GetStudentByIdAsync_ThrowsNotFoundException_WhenStudentDoesNotExist()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _service.GetStudentByIdAsync(999));
        }

        [Fact]
        public async Task CreateStudentAsync_AddsStudentToDatabase()
        {
            // Arrange
            var newStudentDto = new StudentCreateDto { Name = "New Student" };

            // Act
            var result = await _service.CreateStudentAsync(newStudentDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Student", result.Name);
            Assert.Equal(3, _context.Students.Count());
        }

        [Fact]
        public async Task UpdateStudentAsync_UpdatesExistingStudent()
        {
            // Arrange
            var updateDto = new StudentUpdateDto { Name = "Updated Name" };

            // Act
            await _service.UpdateStudentAsync(1, updateDto);

            // Assert
            var updatedStudent = await _context.Students.FindAsync(1);
            Assert.Equal("Updated Name", updatedStudent!.Name);
        }

        [Fact]
        public async Task UpdateStudentAsync_ThrowsNotFoundException_WhenStudentDoesNotExist()
        {
            // Arrange
            var updateDto = new StudentUpdateDto { Name = "Nonexistent" };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _service.UpdateStudentAsync(999, updateDto));
        }

        [Fact]
        public async Task DeleteStudentAsync_RemovesStudentFromDatabase()
        {
            // Act
            await _service.DeleteStudentAsync(1);

            // Assert
            var deletedStudent = await _context.Students.FindAsync(1);
            Assert.Null(deletedStudent);
            Assert.Equal(1, _context.Students.Count());
        }

        [Fact]
        public async Task DeleteStudentAsync_ThrowsNotFoundException_WhenStudentDoesNotExist()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _service.DeleteStudentAsync(999));
        }
    }
}
