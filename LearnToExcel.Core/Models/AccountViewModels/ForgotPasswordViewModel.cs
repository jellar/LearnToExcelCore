using System.ComponentModel.DataAnnotations;

namespace LearnToExcel.Core.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
