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
        public List<CourseInstructor> CourseInstructors { get; set; }
       
    }
}
