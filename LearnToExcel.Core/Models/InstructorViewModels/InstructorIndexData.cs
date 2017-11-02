using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnToExcel.Core.Models.InstructorViewModels
{
    public class InstructorIndexData
    {
        public ICollection<Instructor> Instructors { get; set; }
        public ICollection<Course> Courses { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
       
    }
}
