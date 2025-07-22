using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizProject.Data.Models
{//quiz çözüm sürecini detaylı izleyebilmek için bir UserAnswer modeli ekleyelim. Bu model sayesinde hangi kullanıcı, hangi soruya ne cevap verdi gibi bilgileri saklayabileceksin.
    public class UserAnswer
    {
        public int Id { get; set; }

        public int UserQuizResultId { get; set; }
        public UserQuizResult UserQuizResult { get; set; } = null!;

        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;

        public string SelectedOption { get; set; } = null!; // "A" ya da "B"
        public bool IsCorrect { get; set; }

        public DateTime AnsweredAt { get; set; } = DateTime.Now;
    }
}
