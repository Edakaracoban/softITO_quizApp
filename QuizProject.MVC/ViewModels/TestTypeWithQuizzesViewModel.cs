namespace QuizProject.MVC.ViewModels
{
    public class TestTypeWithQuizzesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<QuizViewModel> Quizzes { get; set; } = new();
    }
    public class QuizViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
