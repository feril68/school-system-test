using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Dto
{
    public class EnrollmentCreateDto
    {
        [Required(ErrorMessage = "Student ID is required")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Class ID is required")]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Enrollment Date is required")]
        public DateTime EnrollmentDate { get; set; }

    }

}
