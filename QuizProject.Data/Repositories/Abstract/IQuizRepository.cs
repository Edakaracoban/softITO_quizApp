using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using QuizProject.Data.Models;

namespace QuizProject.Data.Repositories.Abstract
{
    public interface IQuizRepository : IRepository<Quiz>
    {
        Task<Quiz?> GetQuizWithQuestionsAsync(int id);

        Task<List<Quiz>> GetQuizzesByCategoryAsync(int categoryId);

        Task<List<Quiz>> GetQuizzesByCategoryAsync(int categoryId, int page, int pageSize);

        Task<List<Quiz>> SearchQuizzesAsync(string searchTerm, int? categoryId = null);

        Task<Quiz?> GetQuizWithResultsAsync(int id);

        Task<List<Quiz>> GetActiveQuizzesAsync();
        Task<List<Quiz>> GetQuizzesByCategoryAndTestTypeAsync(int categoryId, int testTypeId);

    }
}
