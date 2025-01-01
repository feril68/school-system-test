using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SchoolSystem.Models
{
    public class Class
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public required string Name { get; set; }
        [JsonIgnore]
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }
        [JsonIgnore]
        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
    }
}
