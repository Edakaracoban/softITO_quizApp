using System.ComponentModel.DataAnnotations;

namespace QuizProject.MVC.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
