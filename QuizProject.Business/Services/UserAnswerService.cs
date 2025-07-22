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
    public class UserAnswerService : IUserAnswerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserAnswerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<UserAnswer?> GetByIdAsync(int id)
        {
            return await _unitOfWork.UserAnswers.GetByIdAsync(id);
        }

        public async Task<List<UserAnswer>> GetAllAsync()
        {
            return await _unitOfWork.UserAnswers.GetAllAsync();
        }

        public async Task UpdateAsync(UserAnswer answer)
        {
            if (answer == null) throw new ArgumentNullException(nameof(answer));

            await _unitOfWork.UserAnswers.UpdateAsync(answer);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<UserAnswer> answers)
        {
            if (answers == null) throw new ArgumentNullException(nameof(answers));

            await _unitOfWork.UserAnswers.RemoveRangeAsync(answers);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<UserAnswer>> GetAnswersByUserQuizResultIdAsync(int userQuizResultId)
        {
            return await _unitOfWork.UserAnswers.GetAnswersByUserQuizResultIdAsync(userQuizResultId);
        }

        public async Task<List<UserAnswer>> GetAnswersByUserAndQuizResultAsync(string userId, int quizId)
        {
            return await _unitOfWork.UserAnswers.GetAnswersByUserAndQuizResultAsync(userId, quizId);
        }

        public async Task<List<UserAnswer>> GetAnswersByQuestionIdAsync(int questionId)
        {
            return await _unitOfWork.UserAnswers.GetAnswersByQuestionIdAsync(questionId);
        }

        public async Task<UserAnswer?> GetUserAnswerForQuestionAsync(string userId, int questionId)
        {
            return await _unitOfWork.UserAnswers.GetUserAnswerForQuestionAsync(userId, questionId);
        }
        public async Task AddUserAnswerAsync(UserAnswer userAnswer)
        {
            if (userAnswer == null) throw new ArgumentNullException(nameof(userAnswer));

            // İstersen burada UserQuizResult kontrolü yapabilirsin, örneğin:
            var userQuizResult = await _unitOfWork.UserQuizResults.GetByIdAsync(userAnswer.UserQuizResultId);
            if (userQuizResult == null)
                throw new Exception("UserQuizResult not found.");

            await _unitOfWork.UserAnswers.CreateAsync(userAnswer);
            await _unitOfWork.SaveAsync();
        }

        public async Task<UserQuizResult> StartQuizAsync(string userId, int quizId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            // Quiz'i veritabanından çekiyoruz
            var quiz = await _unitOfWork.Quizzes.GetByIdAsync(quizId);
            if (quiz == null)
                throw new Exception("Quiz not found.");

            // Quiz'in sorularının sayısını alıyoruz
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
