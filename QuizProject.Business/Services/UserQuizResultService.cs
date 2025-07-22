using QuizProject.Business.Interfaces;
using QuizProject.Data.Models;
using QuizProject.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizProject.Business.Services
{
    public class UserQuizResultService : IUserQuizResultService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserQuizResultService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<UserQuizResult?> GetByIdAsync(int id)
        {
            return await _unitOfWork.UserQuizResults.GetByIdAsync(id);
        }

        public async Task<List<UserQuizResult>> GetAllAsync()
        {
            return await _unitOfWork.UserQuizResults.GetAllAsync();
        }

        public async Task CreateAsync(UserQuizResult result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));

            await _unitOfWork.UserQuizResults.CreateAsync(result);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(UserQuizResult result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));

            await _unitOfWork.UserQuizResults.UpdateAsync(result);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(UserQuizResult result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));

            await _unitOfWork.UserQuizResults.DeleteAsync(result);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<UserQuizResult> results)
        {
            if (results == null) throw new ArgumentNullException(nameof(results));

            await _unitOfWork.UserQuizResults.RemoveRangeAsync(results);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<UserQuizResult>> GetResultsByUserIdAsync(string userId)
        {
            return await _unitOfWork.UserQuizResults.GetResultsByUserIdAsync(userId);
        }

        public async Task<List<UserQuizResult>> GetResultsByQuizIdAsync(int quizId)
        {
            return await _unitOfWork.UserQuizResults.GetResultsByQuizIdAsync(quizId);
        }

        public async Task<UserQuizResult?> GetResultByUserAndQuizAsync(string userId, int quizId)
        {
            return await _unitOfWork.UserQuizResults.GetResultByUserAndQuizAsync(userId, quizId);
        }

        public async Task<List<UserQuizResult>> GetResultsOrderedByDateAsync(int quizId, bool ascending = true)
        {
            return await _unitOfWork.UserQuizResults.GetResultsOrderedByDateAsync(quizId, ascending);
        }

        public async Task<double> GetAverageScoreByUserAsync(string userId)
        {
            return await _unitOfWork.UserQuizResults.GetAverageScoreByUserAsync(userId);
        }
        public async Task<UserQuizResult> StartQuizAsync(string userId, int quizId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            var quiz = await _unitOfWork.Quizzes.GetByIdAsync(quizId);
            if (quiz == null)
                throw new Exception("Quiz not found.");

            var totalQuestions = quiz.Questions?.Count ?? 0;

            var userQuizResult = new UserQuizResult
            {
                UserId = userId,
                QuizId = quizId,
                TakenAt = DateTime.Now,
                CorrectAnswers = 0,
                TotalQuestions = totalQuestions
            };

            await _unitOfWork.UserQuizResults.CreateAsync(userQuizResult);
            await _unitOfWork.SaveAsync();

            return userQuizResult;
        }


    }
}
