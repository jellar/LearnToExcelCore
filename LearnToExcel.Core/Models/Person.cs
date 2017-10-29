using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LearnToExcel.Core.Models
{

    public enum Gender
    {
        Male, Female, Other
    }
    public class Person
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => FirstName + ", " + Surname;

        public Gender Gender { get; set; }
    }
}
