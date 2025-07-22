using System;
namespace QuizProject.Data.Models
{
    public class Question
    {
        public int Id { get; set; }

        public string Text { get; set; } = null!;
        public string OptionA { get; set; } = null!;
        public string OptionB { get; set; } = null!;

        public string CorrectOption { get; set; } = null!; // "A" veya "B"

        public int? QuizId { get; set; }           // Nullable yapıldı
        public Quiz? Quiz { get; set; }
        // Nullable navigation property


        public ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();

    }
}