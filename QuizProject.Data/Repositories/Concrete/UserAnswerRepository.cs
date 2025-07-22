using Microsoft.EntityFrameworkCore;
using QuizProject.Data;
using QuizProject.Data.Models;
using QuizProject.Data.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizProject.Data.Repositories.Concrete
{
    public class UserAnswerRepository : EfCoreGenericRepository<UserAnswer, AppDbContext>, IUserAnswerRepository
    {
        public UserAnswerRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<UserAnswer>> GetAnswersByUserQuizResultIdAsync(int userQuizResultId)
        {
            return await _context.UserAnswers
                .Where(ua => ua.UserQuizResultId == userQuizResultId)
                .Include(ua => ua.Question)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<UserAnswer?> GetUserAnswerForQuestionAsync(string userId, int questionId)
        {
            return await _context.UserAnswers
                .Include(ua => ua.UserQuizResult)
                .AsNoTracking()
                .FirstOrDefaultAsync(ua =>
                    ua.UserQuizResult.UserId == userId &&
                    ua.QuestionId == questionId
                );
        }

        public async Task<List<UserAnswer>> GetAnswersByQuestionIdAsync(int questionId)
        {
            return await _context.UserAnswers
                .Where(ua => ua.QuestionId == questionId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<UserAnswer>> GetAnswersByUserAndQuizResultAsync(string userId, int quizId)
        {
            return await _context.UserAnswers
                .Include(ua => ua.UserQuizResult)
                    .ThenInclude(uqr => uqr.User)
                .Include(ua => ua.UserQuizResult)
                    .ThenInclude(uqr => uqr.Quiz)
                .Include(ua => ua.Question)
                    .ThenInclude(q => q.Quiz) // Eğer question içindeki quiz gerekli ise
                .Where(ua => ua.UserQuizResult.UserId == userId && ua.UserQuizResult.QuizId == quizId)
                .ToListAsync();
        }
    }
}
