using QuizProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizProject.Business.Interfaces
{
    public interface IUserQuizResultService
    {
        Task<UserQuizResult?> GetByIdAsync(int id);
        Task<List<UserQuizResult>> GetAllAsync();

        Task CreateAsync(UserQuizResult result);
        Task UpdateAsync(UserQuizResult result);
        Task DeleteAsync(UserQuizResult result);
        Task RemoveRangeAsync(IEnumerable<UserQuizResult> results);

        Task<List<UserQuizResult>> GetResultsByUserIdAsync(string userId);
        Task<List<UserQuizResult>> GetResultsByQuizIdAsync(int quizId);
        Task<UserQuizResult?> GetResultByUserAndQuizAsync(string userId, int quizId);
        Task<List<UserQuizResult>> GetResultsOrderedByDateAsync(int quizId, bool ascending = true);
        Task<double> GetAverageScoreByUserAsync(string userId);
        Task<UserQuizResult> StartQuizAsync(string userId, int quizId);
    }
}
