using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Data.Model
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title must be between 1 - 100")]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        public ICollection<Enrollment> Enrollments { get; set; } = [];
    }
}
