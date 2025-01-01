using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Dto;
using SchoolSystem.Models;

namespace SchoolSystem.Controllers
{
    [ApiController]
    [Route("api/teachers")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService TeacherService)
        {
            _teacherService = TeacherService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
        {
            var Teachers = await _teacherService.GetAllTeachersAsync();
            return Ok(Teachers);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> GetTeacher(int id)
        {
            var Teacher = await _teacherService.GetTeacherByIdAsync(id);
            return Ok(Teacher);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Teacher>> CreateTeacher(TeacherCreateDto teacherCreateDto)
        {
            var createdTeacher = await _teacherService.CreateTeacherAsync(teacherCreateDto);
            return CreatedAtAction(nameof(GetTeacher), new { id = createdTeacher.Id }, createdTeacher);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeacher(int id, TeacherUpdateDto teacherUpdateDto)
        {
            await _teacherService.UpdateTeacherAsync(id, teacherUpdateDto);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            await _teacherService.DeleteTeacherAsync(id);
            return NoContent();
        }
    }
}
