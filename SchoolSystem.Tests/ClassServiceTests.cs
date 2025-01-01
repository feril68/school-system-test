using Microsoft.EntityFrameworkCore;
using SchoolSystem.Data;
using SchoolSystem.Dto;
using SchoolSystem.Exceptions;
using SchoolSystem.Models;
using SchoolSystem.Services;
using Xunit;

namespace SchoolSystem.Tests
{
    public class ClassServiceTests
    {
        private readonly DbContextOptions<AppDbContext> _options;
        private readonly AppDbContext _context;
        private readonly ClassService _service;

        public ClassServiceTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(_options);
            _service = new ClassService(_context);

            // Seed data
            var subject = new Subject { Id = 1, Name = "Math" };
            var teacher = new Teacher { Id = 1, Name = "Mr. Smith" };

            _context.Subjects.Add(subject);
            _context.Teachers.Add(teacher);

            _context.Classes.AddRange(
                new Class { Id = 1, Name = "Class A", Subject = subject, Teacher = teacher },
                new Class { Id = 2, Name = "Class B", Subject = subject, Teacher = teacher }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllClassesAsync_ReturnsAllClasses()
        {
            // Act
            var result = await _service.GetAllClassesAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetClassByIdAsync_ReturnsClass_WhenClassExists()
        {
            // Act
            var result = await _service.GetClassByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Class A", result.Name);
        }

        [Fact]
        public async Task GetClassByIdAsync_ThrowsNotFoundException_WhenClassDoesNotExist()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _service.GetClassByIdAsync(999));

            Assert.Equal("Class with ID 999 does not exist", exception.Message);
        }

        [Fact]
        public async Task CreateClassAsync_AddsClassToDatabase()
        {
            // Arrange
            var newClassDto = new ClassCreateDto { Name = "New Class", SubjectId = 1, TeacherId = 1 };

            // Act
            var result = await _service.CreateClassAsync(newClassDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Class", result.Name);
            Assert.Equal(3, _context.Classes.Count());
        }

        [Fact]
        public async Task CreateClassAsync_ThrowsNotFoundException_WhenSubjectDoesNotExist()
        {
            // Arrange
            var newClassDto = new ClassCreateDto { Name = "New Class", SubjectId = 999, TeacherId = 1 };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _service.CreateClassAsync(newClassDto));

            Assert.Equal("Subject with ID 999 does not exist", exception.Message);
        }

        [Fact]
        public async Task CreateClassAsync_ThrowsNotFoundException_WhenTeacherDoesNotExist()
        {
            // Arrange
            var newClassDto = new ClassCreateDto { Name = "New Class", SubjectId = 1, TeacherId = 999 };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _service.CreateClassAsync(newClassDto));

            Assert.Equal("Teacher with ID 999 does not exist", exception.Message);
        }

        [Fact]
        public async Task UpdateClassAsync_UpdatesExistingClass()
        {
            // Arrange
            var updateDto = new ClassUpdateDto { Name = "Updated Name", SubjectId = 1, TeacherId = 1 };

            // Act
            await _service.UpdateClassAsync(1, updateDto);

            // Assert
            var updatedClass = await _context.Classes.FindAsync(1);
            Assert.Equal("Updated Name", updatedClass!.Name);
        }

        [Fact]
        public async Task UpdateClassAsync_ThrowsNotFoundException_WhenClassDoesNotExist()
        {
            // Arrange
            var updateDto = new ClassUpdateDto { Name = "Nonexistent", SubjectId = 1, TeacherId = 1 };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _service.UpdateClassAsync(999, updateDto));

            Assert.Equal("Class with ID 999 does not exist", exception.Message);
        }

        [Fact]
        public async Task DeleteClassAsync_RemovesClassFromDatabase()
        {
            // Act
            await _service.DeleteClassAsync(1);

            // Assert
            var deletedClass = await _context.Classes.FindAsync(1);
            Assert.Null(deletedClass);
            Assert.Equal(1, _context.Classes.Count());
        }

        [Fact]
        public async Task DeleteClassAsync_ThrowsNotFoundException_WhenClassDoesNotExist()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _service.DeleteClassAsync(999));

            Assert.Equal("Class with ID 999 does not exist", exception.Message);
        }
    }
}
