using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuizProject.Data.Models;

namespace QuizProject.Data.Repositories.Abstract
{
    public interface IUserQuizResultRepository : IRepository<UserQuizResult>
    {
        // Kullanıcıya ait sonuçları getirir
        Task<List<UserQuizResult>> GetResultsByUserIdAsync(string userId);

        // Quiz’e ait tüm sonuçları getirir
        Task<List<UserQuizResult>> GetResultsByQuizIdAsync(int quizId);

        // Belirli bir kullanıcı ve quiz için sonucu getirir (varsa)
        Task<UserQuizResult?> GetResultByUserAndQuizAsync(string userId, int quizId);

        // Sonuçları tarihe göre sırala
        Task<List<UserQuizResult>> GetResultsOrderedByDateAsync(int quizId, bool ascending = true);

        // Kullanıcının puan ortalaması
        Task<double> GetAverageScoreByUserAsync(string userId);
    }
}
