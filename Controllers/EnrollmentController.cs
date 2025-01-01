using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Dto;
using SchoolSystem.Models;
using SchoolSystem.Services;

namespace SchoolSystem.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetEnrollmentsByStudent(int studentId)
        {
            var enrollments = await _enrollmentService.GetEnrollmentsByStudentAsync(studentId);
            return Ok(enrollments);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Enrollment>> CreateEnrollment(EnrollmentCreateDto enrollmentCreateDto)
        {
            var createdEnrollment = await _enrollmentService.CreateEnrollmentAsync(enrollmentCreateDto);
            return CreatedAtAction(nameof(GetEnrollmentsByStudent), new { studentId = enrollmentCreateDto.StudentId }, createdEnrollment);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            await _enrollmentService.DeleteEnrollmentAsync(id);
            return NoContent();
        }
    }
}
