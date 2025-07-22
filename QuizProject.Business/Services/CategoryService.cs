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
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Categories.GetByIdAsync(id);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _unitOfWork.Categories.GetAllAsync();
        }

        public async Task CreateAsync(Category category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));

            await _unitOfWork.Categories.CreateAsync(category);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));

            await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(Category category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));

            await _unitOfWork.Categories.DeleteAsync(category);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<Category> categories)
        {
            if (categories == null) throw new ArgumentNullException(nameof(categories));

            await _unitOfWork.Categories.RemoveRangeAsync(categories);
            await _unitOfWork.SaveAsync();
        }

        // Repository’den gelen özel metotlar:

        public async Task<Category?> GetCategoryWithQuizzesAsync(int id)
        {
            return await _unitOfWork.Categories.GetCategoryWithQuizzesAsync(id);
        }

        public async Task<int> GetQuizCountByCategoryAsync(int categoryId)
        {
            return await _unitOfWork.Categories.GetQuizCountByCategoryAsync(categoryId);
        }

        public async Task<List<Category>> SearchCategoriesAsync(string searchTerm)
        {
            return await _unitOfWork.Categories.SearchCategoriesAsync(searchTerm);
        }

        public async Task<List<Category>> GetPopularCategoriesAsync(int topCount)
        {
            return await _unitOfWork.Categories.GetPopularCategoriesAsync(topCount);
        }

        public async Task<List<Category>> GetCategoriesAsync(int page, int pageSize)
        {
            return await _unitOfWork.Categories.GetCategoriesAsync(page, pageSize);
        }
    }
}
