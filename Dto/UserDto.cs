using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Dto
{
    public class UserDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

}
