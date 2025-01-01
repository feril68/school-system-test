using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public required string Name { get; set; }

    }
}
