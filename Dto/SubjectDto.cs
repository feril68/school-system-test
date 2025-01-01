using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Dto
{
    public class SubjectCreateDto
    {
        [Required, MaxLength(50)]
        public required string Name { get; set; }
    }

    public class SubjectUpdateDto : SubjectCreateDto { }
}
