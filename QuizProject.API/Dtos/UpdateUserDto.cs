namespace QuizProject.API.Dtos
{
    public class UpdateUserDto
    {
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
