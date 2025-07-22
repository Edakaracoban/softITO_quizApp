using QuizProject.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizProject.Data.Repositories.Abstract
{
    public interface IUserAnswerRepository : IRepository<UserAnswer>
    {
        Task<List<UserAnswer>> GetAnswersByUserQuizResultIdAsync(int userQuizResultId);
        Task<List<UserAnswer>> GetAnswersByUserAndQuizResultAsync(string userId, int quizId);
        Task<List<UserAnswer>> GetAnswersByQuestionIdAsync(int questionId);
        Task<UserAnswer?> GetUserAnswerForQuestionAsync(string userId, int questionId);
    }
}
