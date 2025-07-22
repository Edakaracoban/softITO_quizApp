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
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuestionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Question?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Questions.GetByIdAsync(id);
        }

        public async Task<List<Question>> GetAllAsync()
        {
            return await _unitOfWork.Questions.GetAllAsync();
        }

        public async Task CreateAsync(Question question)
        {
            if (question == null) throw new ArgumentNullException(nameof(question));
            await _unitOfWork.Questions.CreateAsync(question);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(Question question)
        {
            if (question == null) throw new ArgumentNullException(nameof(question));
            await _unitOfWork.Questions.UpdateAsync(question);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(Question question)
        {
            if (question == null) throw new ArgumentNullException(nameof(question));
            await _unitOfWork.Questions.DeleteAsync(question);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<Question> questions)
        {
            if (questions == null) throw new ArgumentNullException(nameof(questions));
            await _unitOfWork.Questions.RemoveRangeAsync(questions);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<Question>> GetQuestionsByQuizIdAsync(int quizId)
        {
            return await _unitOfWork.Questions.GetQuestionsByQuizIdAsync(quizId);
        }

        public async Task<Question?> GetNextQuestionAsync(int quizId, int currentQuestionId)
        {
            return await _unitOfWork.Questions.GetNextQuestionAsync(quizId, currentQuestionId);
        }

        public async Task<Question?> GetPreviousQuestionAsync(int quizId, int currentQuestionId)
        {
            return await _unitOfWork.Questions.GetPreviousQuestionAsync(quizId, currentQuestionId);
        }

        public async Task<List<Question>> SearchQuestionsAsync(string searchText)
        {
            return await _unitOfWork.Questions.SearchQuestionsAsync(searchText);
        }
        public async Task<Quiz?> GetQuizWithQuestionsAsync(int id)
        {
            return await _unitOfWork.Questions.GetQuizWithQuestionsAsync(id);
        }



    }
}
