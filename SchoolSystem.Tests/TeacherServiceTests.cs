using Microsoft.EntityFrameworkCore;
using SchoolSystem.Data;
using SchoolSystem.Dto;
using SchoolSystem.Exceptions;
using SchoolSystem.Models;
using SchoolSystem.Services;
using Xunit;

namespace SchoolSystem.Tests
{
    public class TeacherServiceTests
    {
        private readonly DbContextOptions<AppDbContext> _options;
        private readonly AppDbContext _context;
        private readonly TeacherService _service;

        public TeacherServiceTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(_options);
            _service = new TeacherService(_context);

            // Seed data
            _context.Teachers.AddRange(
                new Teacher { Id = 1, Name = "John Doe", Email = "john.doe@gmail.com", PhoneNumber = "+6281234123123" },
                new Teacher { Id = 2, Name = "Jane Doe", Email = "jane.doe@gmail.com", PhoneNumber = "+6281234123123" }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllTeachersAsync_ReturnsAllTeachers()
        {
            // Act
            var result = await _service.GetAllTeachersAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetTeacherByIdAsync_ReturnsTeacher_WhenTeacherExists()
        {
            // Act
            var result = await _service.GetTeacherByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
        }

        [Fact]
        public async Task GetTeacherByIdAsync_ThrowsNotFoundException_WhenTeacherDoesNotExist()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _service.GetTeacherByIdAsync(999));
        }

        [Fact]
        public async Task CreateTeacherAsync_AddsTeacherToDatabase()
        {
            // Arrange
            var newTeacherDto = new TeacherCreateDto { Name = "New Teacher", Email = "new.teacher@gmail.com", PhoneNumber = "+6281234123121" };

            // Act
            var result = await _service.CreateTeacherAsync(newTeacherDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Teacher", result.Name);
            Assert.Equal(3, _context.Teachers.Count());
        }

        [Fact]
        public async Task UpdateTeacherAsync_UpdatesExistingTeacher()
        {
            // Arrange
            var updateDto = new TeacherUpdateDto { Name = "Updated Name", Email = "update.teacher@gmail.com", PhoneNumber = "+6281234123129" };

            // Act
            await _service.UpdateTeacherAsync(1, updateDto);

            // Assert
            var updatedTeacher = await _context.Teachers.FindAsync(1);
            Assert.Equal("Updated Name", updatedTeacher!.Name);
        }

        [Fact]
        public async Task UpdateTeacherAsync_ThrowsNotFoundException_WhenTeacherDoesNotExist()
        {
            // Arrange
            var updateDto = new TeacherUpdateDto { Name = "Nonexistent", Email = "john.doe@gmail.com", PhoneNumber = "+6281234123123" };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _service.UpdateTeacherAsync(999, updateDto));
        }

        [Fact]
        public async Task DeleteTeacherAsync_RemovesTeacherFromDatabase()
        {
            // Act
            await _service.DeleteTeacherAsync(1);

            // Assert
            var deletedTeacher = await _context.Teachers.FindAsync(1);
            Assert.Null(deletedTeacher);
            Assert.Equal(1, _context.Teachers.Count());
        }

        [Fact]
        public async Task DeleteTeacherAsync_ThrowsNotFoundException_WhenTeacherDoesNotExist()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _service.DeleteTeacherAsync(999));
        }
    }
}
