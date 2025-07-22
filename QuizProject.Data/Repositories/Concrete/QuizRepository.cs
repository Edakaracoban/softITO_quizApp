using Microsoft.EntityFrameworkCore;
using QuizProject.Data;
using QuizProject.Data.Models;
using QuizProject.Data.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizProject.Data.Repositories.Concrete
{
    public class QuizRepository : EfCoreGenericRepository<Quiz, AppDbContext>, IQuizRepository
    {
        public QuizRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Quiz?> GetQuizWithQuestionsAsync(int id)
        {
            return await _context.Quizzes
                .Include(q => q.Category)
     
                .Include(q => q.TestType)
                .Include(q => q.Questions)
                    .ThenInclude(q => q.UserAnswers)
                          .ThenInclude(ua => ua.UserQuizResult)
                .AsNoTracking()
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<List<Quiz>> GetQuizzesByCategoryAsync(int categoryId)
        {
            return await _context.Quizzes
         .Where(q => q.CategoryId == categoryId && q.IsActive)
         .Include(q => q.Category)
         .Include(q => q.TestType)
         .AsNoTracking()
         .ToListAsync();
        }

        public async Task<List<Quiz>> GetQuizzesByCategoryAsync(int categoryId, int page, int pageSize)
        {
            return await _context.Quizzes
                .Where(q => q.CategoryId == categoryId)
                .OrderBy(q => q.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Quiz>> SearchQuizzesAsync(string searchTerm, int? categoryId = null)
        {
            var query = _context.Quizzes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(q => EF.Functions.Like(q.Title, $"%{searchTerm}%"));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(q => q.CategoryId == categoryId.Value);
            }

            return await query
                .OrderBy(q => q.Title)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Quiz?> GetQuizWithResultsAsync(int id)
        {
            return await _context.Quizzes
        .Include(q => q.Category)
        .Include(q => q.TestType)
        .Include(q => q.Questions)
            .ThenInclude(q => q.UserAnswers)
                .ThenInclude(ua => ua.UserQuizResult)
        .AsNoTracking()
        .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<List<Quiz>> GetActiveQuizzesAsync()
        {
            return await _context.Quizzes
                .Where(q => q.IsActive)
                .Include(q => q.Category) // Kategori ilişkisini yükle
                .Include(q => q.TestType) // TestType ilişkisini yükle
               .Include(q => q.Questions)
    .ThenInclude(question => question.UserAnswers)
                .OrderBy(q => q.Title)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Quiz>> GetQuizzesByCategoryAndTestTypeAsync(int categoryId, int testTypeId)
        {
            return await _context.Quizzes
                .Where(q => q.CategoryId == categoryId && q.TestTypeId == testTypeId && q.IsActive)
                .Include(q => q.Category)
                .Include(q => q.TestType)
                .Include(q => q.Questions)
                    .ThenInclude(question => question.UserAnswers)
                .Include(q => q.QuizComments)
                    .ThenInclude(comment => comment.User)
                .AsNoTracking()
                .ToListAsync();
        }



    }
}
