namespace QuizProject.MVC.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }             // Kullanıcı ID
        public string Email { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Role { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsLocked { get; set; }         // Kullanıcı kilitli mi?
        public string ProfileImageUrl { get; set; } // Profil resmi URL’si
    }
}
