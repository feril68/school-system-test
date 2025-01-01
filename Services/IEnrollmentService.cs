using SchoolSystem.Dto;
using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentAsync(int studentId);
        Task<Enrollment> CreateEnrollmentAsync(EnrollmentCreateDto enrollmentCreateDto);
        Task DeleteEnrollmentAsync(int id);
    }
}
