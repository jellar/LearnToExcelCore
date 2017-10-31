using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnToExcel.Core.Models.InstructorViewModels
{
    public class CreateInstructorViewModel : Person
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime HireDate { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
