using System.Collections.Generic;
using System.Threading.Tasks;
using QuizProject.Data.Models;

namespace QuizProject.Data.Repositories.Abstract
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category?> GetCategoryWithQuizzesAsync(int id);

        Task<int> GetQuizCountByCategoryAsync(int categoryId);

        Task<List<Category>> SearchCategoriesAsync(string searchTerm);

        Task<List<Category>> GetPopularCategoriesAsync(int topCount);

        Task<List<Category>> GetCategoriesAsync(int page, int pageSize);
    }
}
