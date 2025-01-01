using SchoolSystem.Dto;
using SchoolSystem.Models;

public interface ITeacherService
{
    Task<IEnumerable<Teacher>> GetAllTeachersAsync();
    Task<Teacher?> GetTeacherByIdAsync(int id);
    Task<Teacher> CreateTeacherAsync(TeacherCreateDto teacherCreateDto);
    Task UpdateTeacherAsync(int id, TeacherUpdateDto teacherUpdateDto);
    Task DeleteTeacherAsync(int id);
}