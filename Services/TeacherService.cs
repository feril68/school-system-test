using Microsoft.EntityFrameworkCore;
using SchoolSystem.Data;
using SchoolSystem.Dto;
using SchoolSystem.Exceptions;
using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly AppDbContext _context;

        public TeacherService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Teacher>> GetAllTeachersAsync()
        {
            return await _context.Teachers.ToListAsync();
        }

        public async Task<Teacher?> GetTeacherByIdAsync(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) throw new NotFoundException($"Teacher with ID {id} does not exist");
            return teacher;
        }

        public async Task<Teacher> CreateTeacherAsync(TeacherCreateDto teacherCreateDto)
        {
            Teacher newTeacher = new Teacher
            {
                Name = teacherCreateDto.Name
            };
            _context.Teachers.Add(newTeacher);
            await _context.SaveChangesAsync();
            return newTeacher;
        }

        public async Task UpdateTeacherAsync(int id, TeacherUpdateDto teacherUpdateDto)
        {

            var existingTeacher = await this.GetTeacherByIdAsync(id);
            if (existingTeacher == null) throw new NotFoundException($"Teacher with ID {id} does not exist");
            existingTeacher.Name = teacherUpdateDto.Name;
            await _context.SaveChangesAsync();

        }

        public async Task DeleteTeacherAsync(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) throw new NotFoundException($"Teacher with ID {id} does not exist");
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
        }
    }
}
