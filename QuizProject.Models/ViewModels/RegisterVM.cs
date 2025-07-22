using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizProject.Models.ViewModels
{
    public class RegisterVM
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        // Gerekirse profil resmi url'si de olabilir
        public string? ProfileImageUrl { get; set; }
    }
}