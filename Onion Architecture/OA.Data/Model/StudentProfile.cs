using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Data.Model
{
    public class StudentProfile
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Bio must be between 1 - 100")]
        [MaxLength(100)]
        public string Bio { get; set; } = string.Empty;
        public Student? Student { get; set; }
    }
}
