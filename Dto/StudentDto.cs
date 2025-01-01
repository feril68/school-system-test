using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Dto
{
    public class StudentCreateDto
    {
        [Required(ErrorMessage = "Name is required"), MaxLength(50, ErrorMessage = "Student name cannot exceed 50 characters.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "Email field is not a valid e-mail address")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required"), Phone(ErrorMessage = "PhoneNumber field is not a valid phone number")]
        public required string PhoneNumber { get; set; }
    }

    public class StudentUpdateDto : StudentCreateDto { }
}
