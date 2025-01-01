using Microsoft.EntityFrameworkCore;
using SchoolSystem.Data;
using SchoolSystem.Dto;
using SchoolSystem.Exceptions;
using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly AppDbContext _context;

        public SubjectService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Subject>> GetAllSubjectsAsync()
        {
            return await _context.Subjects.ToListAsync();
        }

        public async Task<Subject?> GetSubjectByIdAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) throw new NotFoundException($"Subject with ID {id} does not exist");
            return subject;
        }

        public async Task<Subject> CreateSubjectAsync(SubjectCreateDto subjectCreateDto)
        {
            Subject newSubject = new Subject
            {
                Name = subjectCreateDto.Name
            };
            _context.Subjects.Add(newSubject);
            await _context.SaveChangesAsync();
            return newSubject;
        }

        public async Task UpdateSubjectAsync(int id, SubjectUpdateDto subjectUpdateDto)
        {

            var existingSubject = await this.GetSubjectByIdAsync(id);
            if (existingSubject == null) throw new NotFoundException($"Subject with ID {id} does not exist");
            existingSubject.Name = subjectUpdateDto.Name;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubjectAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) throw new NotFoundException($"Subject with ID {id} does not exist");
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
        }
    }
}
