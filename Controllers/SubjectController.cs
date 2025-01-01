using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Dto;
using SchoolSystem.Models;

namespace SchoolSystem.Controllers
{
    [ApiController]
    [Route("api/subjects")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService SubjectService)
        {
            _subjectService = SubjectService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjects()
        {
            var Subjects = await _subjectService.GetAllSubjectsAsync();
            return Ok(Subjects);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Subject>> GetSubject(int id)
        {
            var Subject = await _subjectService.GetSubjectByIdAsync(id);
            return Ok(Subject);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Subject>> CreateSubject(SubjectCreateDto subjectCreateDto)
        {
            var createdSubject = await _subjectService.CreateSubjectAsync(subjectCreateDto);
            return CreatedAtAction(nameof(GetSubject), new { id = createdSubject.Id }, createdSubject);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubject(int id, SubjectUpdateDto subjectUpdateDto)
        {
            await _subjectService.UpdateSubjectAsync(id, subjectUpdateDto);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            await _subjectService.DeleteSubjectAsync(id);
            return NoContent();
        }
    }
}
