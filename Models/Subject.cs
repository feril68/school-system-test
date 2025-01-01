using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Models
{
    public class Subject
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        public required int Credit { get; set; }

    }
}
