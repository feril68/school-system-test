using Microsoft.EntityFrameworkCore;
using SchoolSystem.Data;
using SchoolSystem.Dto;
using SchoolSystem.Exceptions;
using SchoolSystem.Models;
using SchoolSystem.Services;
using Xunit;

namespace SchoolSystem.Tests
{
    public class SubjectServiceTests
    {
        private readonly DbContextOptions<AppDbContext> _options;
        private readonly AppDbContext _context;
        private readonly SubjectService _service;

        public SubjectServiceTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(_options);
            _service = new SubjectService(_context);

            // Seed data
            _context.Subjects.AddRange(
                new Subject { Id = 1, Name = "John Doe" },
                new Subject { Id = 2, Name = "Jane Doe" }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllSubjectsAsync_ReturnsAllSubjects()
        {
            // Act
            var result = await _service.GetAllSubjectsAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetSubjectByIdAsync_ReturnsSubject_WhenSubjectExists()
        {
            // Act
            var result = await _service.GetSubjectByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
        }

        [Fact]
        public async Task GetSubjectByIdAsync_ThrowsNotFoundException_WhenSubjectDoesNotExist()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _service.GetSubjectByIdAsync(999));
        }

        [Fact]
        public async Task CreateSubjectAsync_AddsSubjectToDatabase()
        {
            // Arrange
            var newSubjectDto = new SubjectCreateDto { Name = "New Subject" };

            // Act
            var result = await _service.CreateSubjectAsync(newSubjectDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Subject", result.Name);
            Assert.Equal(3, _context.Subjects.Count());
        }

        [Fact]
        public async Task UpdateSubjectAsync_UpdatesExistingSubject()
        {
            // Arrange
            var updateDto = new SubjectUpdateDto { Name = "Updated Name" };

            // Act
            await _service.UpdateSubjectAsync(1, updateDto);

            // Assert
            var updatedSubject = await _context.Subjects.FindAsync(1);
            Assert.Equal("Updated Name", updatedSubject!.Name);
        }

        [Fact]
        public async Task UpdateSubjectAsync_ThrowsNotFoundException_WhenSubjectDoesNotExist()
        {
            // Arrange
            var updateDto = new SubjectUpdateDto { Name = "Nonexistent" };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _service.UpdateSubjectAsync(999, updateDto));
        }

        [Fact]
        public async Task DeleteSubjectAsync_RemovesSubjectFromDatabase()
        {
            // Act
            await _service.DeleteSubjectAsync(1);

            // Assert
            var deletedSubject = await _context.Subjects.FindAsync(1);
            Assert.Null(deletedSubject);
            Assert.Equal(1, _context.Subjects.Count());
        }

        [Fact]
        public async Task DeleteSubjectAsync_ThrowsNotFoundException_WhenSubjectDoesNotExist()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _service.DeleteSubjectAsync(999));
        }
    }
}
