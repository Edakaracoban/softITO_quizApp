using Microsoft.EntityFrameworkCore;
using QuizProject.Data;
using QuizProject.Data.Models;
using QuizProject.Data.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizProject.Data.Repositories.Concrete
{
    public class UserQuizResultRepository : EfCoreGenericRepository<UserQuizResult, AppDbContext>, IUserQuizResultRepository
    {
        public UserQuizResultRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<UserQuizResult>> GetResultsByUserIdAsync(string userId)
        {
            return await _context.UserQuizResults
         .Include(uqr => uqr.Quiz)
             .ThenInclude(q => q.Category)
         .Include(uqr => uqr.Quiz)
             .ThenInclude(q => q.TestType)
         .Include(uqr => uqr.User)
         .Where(uqr => uqr.UserId == userId)
         .ToListAsync();
        }

        public async Task<List<UserQuizResult>> GetResultsByQuizIdAsync(int quizId)
        {
            return await _context.UserQuizResults
                .Where(uqr => uqr.QuizId == quizId)
    
             .Include(uqr => uqr.Quiz)
             .ThenInclude(q => q.Category)
              .Include(uqr => uqr.Quiz)
             .ThenInclude(q => q.TestType)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<UserQuizResult?> GetResultByUserAndQuizAsync(string userId, int quizId)
        {
            return await _context.UserQuizResults
                  .Include(uqr => uqr.User)
         .Where(uqr => uqr.UserId == userId)
             .Include(uqr => uqr.Quiz)
             .ThenInclude(q => q.Category)
              .Include(uqr => uqr.Quiz)
             .ThenInclude(q => q.TestType)
                .AsNoTracking()
                .FirstOrDefaultAsync(uqr => uqr.UserId == userId && uqr.QuizId == quizId);
        }

        public async Task<List<UserQuizResult>> GetResultsOrderedByDateAsync(int quizId, bool ascending = true)
        {
            var query = _context.UserQuizResults
                .Where(uqr => uqr.QuizId == quizId)
                .AsNoTracking();

            return ascending
                ? await query.OrderBy(uqr => uqr.TakenAt).ToListAsync()
                : await query.OrderByDescending(uqr => uqr.TakenAt).ToListAsync();
        }

        public async Task<double> GetAverageScoreByUserAsync(string userId)
        {
            return await _context.UserQuizResults
                .Where(uqr => uqr.UserId == userId)
                .Select(uqr => uqr.Score)
                .DefaultIfEmpty(0)
                .AverageAsync();
        }
    }
}
