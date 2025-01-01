using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Dto
{
    public class SubjectCreateDto
    {
        [Required, MaxLength(50, ErrorMessage = "Class name cannot exceed 50 characters.")]
        public required string Name { get; set; }

        [Required]
        public required int Credit { get; set; }
    }

    public class SubjectUpdateDto : SubjectCreateDto { }
}
