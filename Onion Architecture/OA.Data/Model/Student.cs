using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Data.Model
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name must be between 1 - 100")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        //[MaxLength(200)]
        public StudentProfile? Profile { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = [];
    }
}
