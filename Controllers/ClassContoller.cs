using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Dto;
using SchoolSystem.Models;

namespace SchoolSystem.Controllers
{
    [ApiController]
    [Route("api/classes")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> GetClasses()
        {
            var classes = await _classService.GetAllClassesAsync();
            return Ok(classes);
        }


        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Class>> GetClass(int id)
        {
            var classObj = await _classService.GetClassByIdAsync(id);
            return Ok(classObj);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Class>> CreateClass(ClassCreateDto classCreateDto)
        {
            var createdClass = await _classService.CreateClassAsync(classCreateDto);
            return CreatedAtAction(nameof(GetClass), new { id = createdClass.Id }, createdClass);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClass(int id, ClassUpdateDto classUpdateDto)
        {
            await _classService.UpdateClassAsync(id, classUpdateDto);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            await _classService.DeleteClassAsync(id);
            return NoContent();
        }

    }
}
