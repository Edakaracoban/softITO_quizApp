namespace QuizProject.API.Dtos
{
    public class UpdateUserProfileDto
    {
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string ProfileImageBase64 { get; set; } // Base64 encoded image
    }
}
