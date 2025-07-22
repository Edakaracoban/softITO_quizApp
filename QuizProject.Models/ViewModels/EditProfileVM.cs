using System;
namespace QuizProject.Models.ViewModels
{
    public class EditProfileVM
    {
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}

