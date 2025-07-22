using QuizProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizProject.Business.Interfaces
{
    //Burada amaç repository'deki fonksiyonları servis düzeyinde soyutlamak ve async hale getirmek.
    public interface ICategoryService
    {
        Task<Category?> GetByIdAsync(int id);
        Task<List<Category>> GetAllAsync();
        Task CreateAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Category category);
        Task RemoveRangeAsync(IEnumerable<Category> categories);

        // Repository’den gelen özel metotlar
        Task<Category?> GetCategoryWithQuizzesAsync(int id);
        Task<int> GetQuizCountByCategoryAsync(int categoryId);
        Task<List<Category>> SearchCategoriesAsync(string searchTerm);
        Task<List<Category>> GetPopularCategoriesAsync(int topCount);
        Task<List<Category>> GetCategoriesAsync(int page, int pageSize);
    }
}
