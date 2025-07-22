using System.ComponentModel.DataAnnotations;

namespace QuizProject.API.Dtos
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
