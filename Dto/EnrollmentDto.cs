using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Dto
{
    public class EnrollmentCreateDto
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int ClassId { get; set; }

        [Required]
        public DateTime EnrollmentDate { get; set; }

    }

}
