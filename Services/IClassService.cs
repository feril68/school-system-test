using SchoolSystem.Dto;
using SchoolSystem.Models;

public interface IClassService
{
    Task<IEnumerable<Class>> GetAllClassesAsync();
    Task<Class?> GetClassByIdAsync(int id);
    Task<Class> CreateClassAsync(ClassCreateDto classCreateDto);
    Task UpdateClassAsync(int id, ClassUpdateDto classUpdateDto);
    Task DeleteClassAsync(int id);
}