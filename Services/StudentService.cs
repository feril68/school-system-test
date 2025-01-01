using Microsoft.EntityFrameworkCore;
using SchoolSystem.Data;
using SchoolSystem.Dto;
using SchoolSystem.Exceptions;
using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) throw new NotFoundException($"Student with ID {id} does not exist");
            return student;
        }

        public async Task<Student> CreateStudentAsync(StudentCreateDto studentCreateDto)
        {
            Student newStudent = new Student
            {
                Name = studentCreateDto.Name,
                Email = studentCreateDto.Email,
                PhoneNumber = studentCreateDto.PhoneNumber
            };
            _context.Students.Add(newStudent);
            await _context.SaveChangesAsync();
            return newStudent;
        }

        public async Task UpdateStudentAsync(int id, StudentUpdateDto studentUpdateDto)
        {

            var existingStudent = await this.GetStudentByIdAsync(id);
            if (existingStudent == null) throw new NotFoundException($"Student with ID {id} does not exist");
            existingStudent.Name = studentUpdateDto.Name;
            existingStudent.Email = studentUpdateDto.Email;
            existingStudent.PhoneNumber = studentUpdateDto.PhoneNumber;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) throw new NotFoundException($"Student with ID {id} does not exist");

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }
    }
}
