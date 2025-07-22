using QuizProject.Data.Models;

namespace QuizProject.MVC.ViewModels
{
    public class DashboardViewModel
    {
        // Zaten olanlar
        public int TotalCategories { get; set; }
        public int TotalQuizzes { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalUsers { get; set; }
        public int TotalUserQuizResults { get; set; }
        public List<CategorySummaryViewModel> PopularCategories { get; set; }
        public List<QuestionViewModel> RecentQuestions { get; set; }
        public List<QuestionViewModel> AllQuestions { get; set; }
        public List<TestTypeWithQuizzesViewModel> TestTypesWithQuizzes { get; set; }

        // Yeni eklenenler - Chart verileri
        public List<string> ChartLabels { get; set; }
        public List<int> ChartData { get; set; }
    }
 
}
