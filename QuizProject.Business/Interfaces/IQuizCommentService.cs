using QuizProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizProject.Business.Interfaces
{
    public interface IQuizCommentService
    {
        Task<QuizComment?> GetByIdAsync(int id);
        Task<List<QuizComment>> GetAllAsync();
        Task CreateAsync(QuizComment comment);
        Task UpdateAsync(QuizComment comment);
        Task DeleteAsync(QuizComment comment);
        Task RemoveRangeAsync(IEnumerable<QuizComment> comments);

        // Özel metotlar
        Task<List<QuizComment>> GetCommentsByQuizIdAsync(int quizId);
        Task<List<QuizComment>> GetCommentsByUserIdAsync(string userId);
    }
}
