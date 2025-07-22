using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace QuizProject.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? ProfileImageUrl { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public ICollection<QuizComment> QuizComments { get; set; } = new List<QuizComment>();
        public virtual ICollection<UserQuizResult> UserQuizResults { get; set; } = new List<UserQuizResult>();
    }
}
