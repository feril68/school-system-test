using SchoolSystem.Models;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Data;
using SchoolSystem.Dto;
using SchoolSystem.Exceptions;

namespace SchoolSystem.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly AppDbContext _context;

        public EnrollmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentAsync(int studentId)
        {
            return await _context.Enrollments
                .Include(s => s.Student)
                .Include(e => e.Class)
                .ThenInclude(c => c.Subject)
                .Include(e => e.Class.Teacher)
                .Where(e => e.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<Enrollment> CreateEnrollmentAsync(EnrollmentCreateDto enrollmentCreateDto)
        {
            var existEnrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.ClassId == enrollmentCreateDto.ClassId && e.StudentId == enrollmentCreateDto.StudentId);

            if (existEnrollment != null)
            {
                throw new BadRequestException("Enrollment already exists for the given class and student");
            }

            var classObj = await _context.Classes
                .Include(c => c.Subject)
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(c => c.Id == enrollmentCreateDto.ClassId);

            if (classObj == null)
            {
                throw new NotFoundException($"Class with ID {enrollmentCreateDto.ClassId} does not exist");
            }

            var student = await _context.Students.FindAsync(enrollmentCreateDto.StudentId);
            if (student == null)
            {
                throw new NotFoundException($"Student with ID {enrollmentCreateDto.StudentId} does not exist");
            }

            var newEnrollment = new Enrollment
            {
                Student = student,
                Class = classObj,
                EnrollmentDate = enrollmentCreateDto.EnrollmentDate
            };

            _context.Enrollments.Add(newEnrollment);
            await _context.SaveChangesAsync();
            return newEnrollment;
        }


        public async Task DeleteEnrollmentAsync(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null) throw new NotFoundException($"Enrollment with ID {id} does not exist"); ;

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
        }
    }
}
