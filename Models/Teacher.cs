using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public required string Name { get; set; }

    }
}
