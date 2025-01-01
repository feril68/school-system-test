using SchoolSystem.Dto;
using SchoolSystem.Models;

public interface ISubjectService
{
    Task<IEnumerable<Subject>> GetAllSubjectsAsync();
    Task<Subject?> GetSubjectByIdAsync(int id);
    Task<Subject> CreateSubjectAsync(SubjectCreateDto subjectCreateDto);
    Task UpdateSubjectAsync(int id, SubjectUpdateDto subjectUpdateDto);
    Task DeleteSubjectAsync(int id);
}