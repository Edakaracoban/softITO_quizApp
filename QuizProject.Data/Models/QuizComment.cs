using System;
using System.ComponentModel.DataAnnotations;

namespace QuizProject.Data.Models
{
    public class QuizComment
    {
        public int Id { get; set; }

        // Kullanıcı opsiyonel
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        // Quiz'e bağlı, zorunlu
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;

        [Required]
        public string CommentText { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Opsiyonel kullanıcı quiz sonucu
        public int? UserQuizResultId { get; set; }
        public UserQuizResult? UserQuizResult { get; set; }

        public string UserFullName => User?.FullName ?? "Anonim";
    }
}
