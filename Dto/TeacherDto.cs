using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Dto
{
    public class TeacherCreateDto
    {
        [Required, MaxLength(50, ErrorMessage = "Student name cannot exceed 50 characters.")]
        public required string Name { get; set; }
    }

    public class TeacherUpdateDto : TeacherCreateDto { }
}
