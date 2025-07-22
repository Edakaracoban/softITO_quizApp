namespace QuizProject.MVC.ViewModels
{
    public class QuizzViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? CategoryName { get; set; }
        public string? TestTypeName { get; set; }
        public bool IsActive { get; set; }
        public int QuestionCount { get; set; }
    }
}
