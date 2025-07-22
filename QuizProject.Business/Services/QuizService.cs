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
    public class QuizService : IQuizService
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuizService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Quiz?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Quizzes.GetByIdAsync(id);
        }

        public async Task<List<Quiz>> GetAllAsync()
        {
            return await _unitOfWork.Quizzes.GetAllAsync(null, true, q => q.Category, q => q.TestType,q=>q.Questions);

        }

        public async Task CreateAsync(Quiz quiz)
        {
            if (quiz == null) throw new ArgumentNullException(nameof(quiz));
            await _unitOfWork.Quizzes.CreateAsync(quiz);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(Quiz quiz)
        {
            if (quiz == null) throw new ArgumentNullException(nameof(quiz));
            await _unitOfWork.Quizzes.UpdateAsync(quiz);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(Quiz quiz)
        {
            if (quiz == null) throw new ArgumentNullException(nameof(quiz));
            await _unitOfWork.Quizzes.DeleteAsync(quiz);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<Quiz> quizzes)
        {
            if (quizzes == null) throw new ArgumentNullException(nameof(quizzes));
            await _unitOfWork.Quizzes.RemoveRangeAsync(quizzes);
            await _unitOfWork.SaveAsync();
        }

        // Özelleştirilmiş repository metotları
        public async Task<Quiz?> GetQuizWithQuestionsAsync(int id)
        {
            return await _unitOfWork.Quizzes.GetQuizWithQuestionsAsync(id);
        }

        public async Task<List<Quiz>> GetQuizzesByCategoryAsync(int categoryId)
        {
            return await _unitOfWork.Quizzes.GetQuizzesByCategoryAsync(categoryId);
        }

        public async Task<List<Quiz>> GetQuizzesByCategoryAsync(int categoryId, int page, int pageSize)
        {
            return await _unitOfWork.Quizzes.GetQuizzesByCategoryAsync(categoryId, page, pageSize);
        }

        public async Task<List<Quiz>> SearchQuizzesAsync(string searchTerm, int? categoryId = null)
        {
            return await _unitOfWork.Quizzes.SearchQuizzesAsync(searchTerm, categoryId);
        }

        public async Task<Quiz?> GetQuizWithResultsAsync(int id)
        {
            return await _unitOfWork.Quizzes.GetQuizWithResultsAsync(id);
        }

        public async Task<List<Quiz>> GetActiveQuizzesAsync()
        {
            return await _unitOfWork.Quizzes.GetActiveQuizzesAsync();
        }
        public async Task<List<Quiz>> GetQuizzesByTestTypeAsync(int testTypeId)
        {
            return await _unitOfWork.Quizzes.GetAllAsync(
               q => q.TestTypeId == testTypeId && q.IsActive,
               true,
               q => q.Category,
               q => q.TestType
           );


        }
        public async Task<List<Quiz>> GetQuizzesByCategoryAndTestTypeAsync(int categoryId, int testTypeId)
        {
            return await _unitOfWork.Quizzes.GetQuizzesByCategoryAndTestTypeAsync(categoryId, testTypeId);
        }



    }
}
