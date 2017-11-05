using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnToExcel.Core.Models
{
    public class Instructor : Person
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime HireDate { get; set; }

        [Required]
        public string Email { get; set; }

        public bool Active { get; set; }

        public DateTime EndDate { get; set; }

        [NotMapped]
        public ICollection<Course> Courses { get; set; }

        [NotMapped]
        public string CoursesList { get; set; }

    }
}
