using SchoolSystem.Models;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Data;
using SchoolSystem.Dto;
using SchoolSystem.Exceptions;

namespace SchoolSystem.Services
{
    public class ClassService : IClassService
    {
        private readonly AppDbContext _context;

        public ClassService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Class>> GetAllClassesAsync()
        {
            return await _context.Classes.Include(c => c.Subject).Include(c => c.Teacher).ToListAsync();
        }

        public async Task<Class?> GetClassByIdAsync(int id)
        {
            var classObj = await _context.Classes.Include(c => c.Subject).Include(c => c.Teacher).FirstOrDefaultAsync(c => c.Id == id);
            if (classObj == null) throw new NotFoundException($"Class with ID {id} does not exist");
            return classObj;
        }

        public async Task<Class> CreateClassAsync(ClassCreateDto classCreateDto)
        {
            var subject = await _context.Subjects.FindAsync(classCreateDto.SubjectId);
            if (subject == null)
            {
                throw new NotFoundException($"Subject with ID {classCreateDto.SubjectId} does not exist");
            }

            var teacher = await _context.Teachers.FindAsync(classCreateDto.TeacherId);
            if (teacher == null)
            {
                throw new NotFoundException($"Teacher with ID {classCreateDto.TeacherId} does not exist");
            }

            Class newClass = new Class
            {
                Name = classCreateDto.Name,
                Subject = subject,
                Teacher = teacher
            };
            _context.Classes.Add(newClass);
            await _context.SaveChangesAsync();
            return newClass;

        }

        public async Task UpdateClassAsync(int id, ClassUpdateDto classUpdateDto)
        {
            var existingStudent = await GetClassByIdAsync(id);
            var subject = await _context.Subjects.FindAsync(classUpdateDto.SubjectId);
            if (subject == null)
            {
                throw new NotFoundException($"Subject with ID {classUpdateDto.SubjectId} does not exist");
            }

            var teacher = await _context.Teachers.FindAsync(classUpdateDto.TeacherId);
            if (teacher == null)
            {
                throw new NotFoundException($"Teacher with ID {classUpdateDto.TeacherId} does not exist");
            }
            if (existingStudent == null) throw new NotFoundException($"Class with ID {id} does not exist");
            existingStudent.Name = classUpdateDto.Name;
            existingStudent.SubjectId = classUpdateDto.SubjectId;
            existingStudent.TeacherId = classUpdateDto.TeacherId;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClassAsync(int id)
        {
            var classObj = await _context.Classes.FindAsync(id);
            if (classObj == null) throw new NotFoundException($"Class with ID {id} does not exist");
            _context.Classes.Remove(classObj);
            await _context.SaveChangesAsync();
        }
    }
}
