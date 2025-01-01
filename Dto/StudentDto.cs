using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Dto
{
    public class StudentCreateDto
    {
        [Required, MaxLength(50, ErrorMessage = "Student name cannot exceed 50 characters.")]
        public required string Name { get; set; }
    }

    public class StudentUpdateDto : StudentCreateDto { }
}
