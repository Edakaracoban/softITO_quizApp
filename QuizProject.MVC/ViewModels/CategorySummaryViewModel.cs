using QuizProject.Data.Models;

namespace QuizProject.MVC.ViewModels
{
    public class CategorySummaryViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int QuizCount { get; set; }
        public int TotalQuizAttempts { get; set; }
        public int TotalQuestions { get; set; }
        public List<Question> RecentQuestions { get; set; }  // Burada Question entity ya da ViewModel olabilir
    }
}
