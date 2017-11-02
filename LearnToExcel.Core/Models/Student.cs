using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LearnToExcel.Core.Models
{
    public class Student : Person
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Start Date")]
        public DateTime EnrolmentDate { get; set; }

        [Display(Name = "Name of School")]
        public string SchoolName { get; set; }

        [Display(Name = "Year at School")]
        public int ShchoolYear { get; set; }

        [Display(Name = "Any known medical conditions")]
        public string KnownMedicalConditions { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        [Display(Name = "Monthly Payment Day")]
        public int PaymentDay { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public int ParentId { get; set; }
        public virtual Parent Parent { get; set; }

    }

}
