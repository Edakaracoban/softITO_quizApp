using QuizProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizProject.Business.Interfaces
{
    public interface IQuizService
    {
        Task<Quiz?> GetByIdAsync(int id);
        Task<List<Quiz>> GetAllAsync();
        Task CreateAsync(Quiz quiz);
        Task UpdateAsync(Quiz quiz);
        Task DeleteAsync(Quiz quiz);
        Task RemoveRangeAsync(IEnumerable<Quiz> quizzes);

        // Repository'den gelen özel metotlar
        Task<Quiz?> GetQuizWithQuestionsAsync(int id);
        Task<List<Quiz>> GetQuizzesByCategoryAsync(int categoryId);
        Task<List<Quiz>> GetQuizzesByCategoryAsync(int categoryId, int page, int pageSize);
        Task<List<Quiz>> SearchQuizzesAsync(string searchTerm, int? categoryId = null);
        Task<Quiz?> GetQuizWithResultsAsync(int id);
        Task<List<Quiz>> GetActiveQuizzesAsync();
        Task<List<Quiz>> GetQuizzesByTestTypeAsync(int testTypeId);
        Task<List<Quiz>> GetQuizzesByCategoryAndTestTypeAsync(int categoryId, int testTypeId);

    }
}
