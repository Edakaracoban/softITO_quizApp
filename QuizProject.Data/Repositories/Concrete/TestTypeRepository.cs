using Microsoft.EntityFrameworkCore;
using QuizProject.Data;
using QuizProject.Data.Models;
using QuizProject.Data.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizProject.Data.Repositories.Concrete
{
    public class TestTypeRepository : EfCoreGenericRepository<TestType, AppDbContext>, ITestTypeRepository
    {
        public TestTypeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<TestType>> GetTestTypesWithQuizzesAsync()
        {
            return await _context.TestTypes
                .Include(tt => tt.Quizzes)
                    .ThenInclude(q => q.Category)        // Category'yi ekle
         
                .Where(tt => tt.Quizzes.Any(q => q.IsActive)) // Sadece aktif quizleri olan test türleri

                .ToListAsync();
        }
        public async Task<TestType?> GetTestTypeByNameAsync(string? term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return null;

            term = term.Trim().ToLower();

            return await _context.TestTypes
                .Include(tt => tt.Quizzes)
                    .ThenInclude(q => q.Category)
                .FirstOrDefaultAsync(tt => tt.Name.ToLower().Contains(term));
        }
        public async Task<List<TestType>> SearchTestTypesAsync(string? term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return await _context.TestTypes
                    .Include(tt => tt.Quizzes)
                        .ThenInclude(q => q.Category)
                    .ToListAsync();
            }

            term = term.Trim().ToLower();

            return await _context.TestTypes
                .Include(tt => tt.Quizzes)
                    .ThenInclude(q => q.Category)
                .Where(tt => tt.Name.ToLower().Contains(term))
                .ToListAsync();
        }
        public async Task<List<TestType>> GetTestTypesByCategoryAsync(int categoryId)
        {
            return await _context.Quizzes
                .Where(q => q.CategoryId == categoryId && q.TestTypeId != null)
                .Select(q => q.TestType!)
                .Distinct()
                .ToListAsync();
        }




    }
}
