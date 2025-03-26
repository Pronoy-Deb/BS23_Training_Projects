using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Data.Model
{
    public class Enrollment
    {
        [Key]
        public int StudentId { get; set; }
        public Student? Student { get; set; }
        [Key]
        public int CourseId { get; set; }
        public Course? Course { get; set; }
    }
}
