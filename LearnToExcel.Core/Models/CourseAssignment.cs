using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LearnToExcel.Core.Models
{
    public class CourseAssignment
    {
        [Key]
        public int Id { get; set; }

        public int InstructorId { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}
