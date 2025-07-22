using System;
namespace QuizProject.Data.Models
{
    public class UserQuizResult
    {
        public int Id { get; set; }

        public string? UserId { get; set; } // Nullable yapıldı
        public ApplicationUser? User { get; set; }

        public int? QuizId { get; set; } // Nullable yapıldı
        public Quiz? Quiz { get; set; }
        


        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }

        public DateTime TakenAt { get; set; } = DateTime.Now;

        public int Score => (int)Math.Round((CorrectAnswers * 100.0) / TotalQuestions);
        public ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
        public ICollection<QuizComment> QuizComments { get; set; } = new List<QuizComment>();

    }
}

