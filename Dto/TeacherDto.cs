using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Dto
{
    public class TeacherCreateDto
    {
        [Required, MaxLength(50)]
        public required string Name { get; set; }
    }

    public class TeacherUpdateDto : TeacherCreateDto { }
}
