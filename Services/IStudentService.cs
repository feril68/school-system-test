using SchoolSystem.Dto;
using SchoolSystem.Models;

public interface IStudentService
{
    Task<IEnumerable<Student>> GetAllStudentsAsync();
    Task<Student?> GetStudentByIdAsync(int id);
    Task<Student> CreateStudentAsync(StudentCreateDto studentCreateDto);
    Task UpdateStudentAsync(int id, StudentUpdateDto studentUpdateDto);
    Task DeleteStudentAsync(int id);
}