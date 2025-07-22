using QuizProject.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizProject.Data.Repositories.Abstract
{
    public interface IQuizCommentRepository : IRepository<QuizComment>
    {
        Task<List<QuizComment>> GetCommentsByQuizIdAsync(int quizId);
        Task<List<QuizComment>> GetCommentsByUserIdAsync(string userId);
    }
}
