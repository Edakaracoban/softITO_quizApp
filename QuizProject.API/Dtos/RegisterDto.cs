using System.ComponentModel.DataAnnotations;

namespace QuizProject.API.Dtos
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        public DateTime BirthDate { get; set; }

        public string? ProfileImageUrl { get; set; }

        [Required]
        [RegularExpression("Admin|Teacher|Student", ErrorMessage = "Role must be Admin, Teacher or Student")]
        public string Role { get; set; } = null!;
    }
}
