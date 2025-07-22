using System.Collections.Generic;

namespace QuizProject.Data.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public int? TestTypeId { get; set; }
        public TestType? TestType { get; set; }

        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<UserQuizResult> UserQuizResults { get; set; } = new List<UserQuizResult>();
        public ICollection<QuizComment> QuizComments { get; set; } = new List<QuizComment>(); // ✅ DÜZGÜN AD
        public bool IsActive { get; set; } = true; // varsayılan olarak aktif

    }
}
