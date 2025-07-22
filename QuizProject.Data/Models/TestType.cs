using System;
namespace QuizProject.Data.Models
{
    public class TestType
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; } // Test görseli
        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    }
}