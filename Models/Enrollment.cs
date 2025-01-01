using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SchoolSystem.Models
{
    public class Enrollment
    {
        public int Id { get; set; }

        [JsonIgnore]
        public int StudentId { get; set; }
        [Required]
        public required Student Student { get; set; }

        [JsonIgnore]
        public int ClassId { get; set; }
        [Required]
        public required Class Class { get; set; }

        [Required]
        public DateTime EnrollmentDate { get; set; }
    }
}
