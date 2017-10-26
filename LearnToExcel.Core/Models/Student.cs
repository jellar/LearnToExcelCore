﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnToExcel.Core.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Surname")]
        public string Surname { get; set; }
        public Gender Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrolmentDate { get; set; }

        [Display(Name = "Name of School")]
        public string SchoolName { get; set; }

        [Display(Name = "Year at School")]
        public int ShchoolYear { get; set; }

        [Display(Name = "Any known medical conditions")]
        public string KnownMedicalConditions { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }



        public ICollection<Enrollment> Enrollments { get; set; }

        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public int ParentId { get; set; }
        public virtual Parent Parent { get; set; }

    }

    public enum Gender
    {
        Male, Female, Other
    }
}
