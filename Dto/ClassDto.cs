using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Dto
{
    public class ClassCreateDto
    {
        [Required(ErrorMessage = "Class name is required."), MaxLength(50, ErrorMessage = "Class name cannot exceed 50 characters.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Subject ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "SubjectId must be a positive number.")]
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "Teacher ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "SubjectId must be a positive number.")]
        public int TeacherId { get; set; }

    }

    public class ClassUpdateDto : ClassCreateDto { }
}
