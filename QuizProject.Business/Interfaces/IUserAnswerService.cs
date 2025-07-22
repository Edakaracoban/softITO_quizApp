using QuizProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizProject.Business.Interfaces
{
    public interface IUserAnswerService
    {
        Task<List<UserAnswer>> GetAllAsync();
        Task<UserAnswer?> GetByIdAsync(int id);
        Task UpdateAsync(UserAnswer answer);
        Task RemoveRangeAsync(IEnumerable<UserAnswer> answers);
        Task<List<UserAnswer>> GetAnswersByUserQuizResultIdAsync(int userQuizResultId);
        Task<List<UserAnswer>> GetAnswersByUserAndQuizResultAsync(string userId, int quizId);
        Task<List<UserAnswer>> GetAnswersByQuestionIdAsync(int questionId);
        Task<UserAnswer?> GetUserAnswerForQuestionAsync(string userId, int questionId);
        Task AddUserAnswerAsync(UserAnswer userAnswer);
        Task<UserQuizResult> StartQuizAsync(string userId, int quizId);

    }
}
