using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnToExcel.Core.Models.AdmissionViewModels
{
    public class StudentRegistration
    {
        [Required(ErrorMessage = "Student First Name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required (ErrorMessage = "Student Surname is required")]
        public string Surname { get; set; }

        [Required (ErrorMessage = "Student Date of Birth is required")]
        [Display(Name = "Date Of Birth")]
        public DateTime DateofBirth { get; set; }

        public ParentView Parent { get; set; }
        
        [Display(Name = "Mother's Phone")]
        public string PrimaryContact { get; set; }

        [Display(Name = "Father's Phone")]
        public string SecondaContact { get; set; }

        [Required]
        [Display(Name = "Parent Email")]
        public string ParentsEmail { get; set; }

        [Display(Name = "Name of School")]
        public string SchoolName { get; set; }
        [Display(Name = "Year at School")]
        public int YearAtSchool { get; set; }
    
        [Display(Name = "Any known medical conditions")]
        public string MedicalConditions { get; set; }
    }

    public class ParentView
    {
        [Required(ErrorMessage = "Parent title is required")]
        public Title Title { get; set; }
        [Required(ErrorMessage = "Parent First Name is required")]
        [Display(Name="First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Parent Surname is required")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Post code is required")]
        [Display(Name = "Post Code")]
        public string PostalCode { get; set; }
    }
}
