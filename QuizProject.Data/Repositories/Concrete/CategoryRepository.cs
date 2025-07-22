using Microsoft.EntityFrameworkCore;
using QuizProject.Data;
using QuizProject.Data.Models;
using QuizProject.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizProject.Data.Repositories.Concrete
{
    public class CategoryRepository : EfCoreGenericRepository<Category, AppDbContext>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Category?> GetCategoryWithQuizzesAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Quizzes)
                    .ThenInclude(q => q.TestType)          // TestType'ı dahil et
                .Include(c => c.Quizzes)
                    .ThenInclude(q => q.QuizComments)      // QuizComments dahil
                .Include(c => c.Quizzes)
                    .ThenInclude(q => q.UserQuizResults)   // UserQuizResults dahil et
                        .ThenInclude(uqr => uqr.User)      // UserQuizResults içinden User dahil et
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int> GetQuizCountByCategoryAsync(int categoryId)
        {
            return await _context.Quizzes
                .CountAsync(q => q.CategoryId == categoryId);
        }

        public async Task<List<Category>> SearchCategoriesAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<Category>();

            return await _context.Categories
                .Where(c => EF.Functions.Like(c.Name, $"%{searchTerm}%"))
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<List<Category>> GetPopularCategoriesAsync(int topCount)
        {
                    return await _context.Categories
             .Include(c => c.Quizzes)
                 .ThenInclude(q => q.TestType) // TestType dahil
             .Include(c => c.Quizzes)
                 .ThenInclude(q => q.UserQuizResults)
                     .ThenInclude(uqr => uqr.User) // User dahil
             .OrderByDescending(c => c.Quizzes.SelectMany(q => q.UserQuizResults).Count())
             .Take(topCount)
             .ToListAsync();
        }

        public async Task<List<Category>> GetCategoriesAsync(int page, int pageSize)
        {
            return await _context.Categories
                .OrderBy(c => c.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
