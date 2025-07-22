using Microsoft.EntityFrameworkCore;
using QuizProject.Data;
using QuizProject.Data.Models;
using QuizProject.Data.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizProject.Data.Repositories.Concrete
{
    public class QuestionRepository : EfCoreGenericRepository<Question, AppDbContext>, IQuestionRepository
    {
        public QuestionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Question>> GetQuestionsByQuizIdAsync(int quizId)
        {
            return await _context.Questions
                .Where(q => q.QuizId == quizId)
                .Include(q => q.Quiz)
                    .ThenInclude(qz => qz.Category) // Kategori bilgisi
                .Include(q => q.Quiz)
                    .ThenInclude(qz => qz.TestType) // Test türü bilgisi
                .Include(q => q.UserAnswers)
                    .ThenInclude(ua => ua.UserQuizResult) // Kullanıcı sonucu
                        .ThenInclude(uqr => uqr.User) // Kullanıcı bilgisi
                .Include(q => q.UserAnswers)
                    .ThenInclude(ua => ua.UserQuizResult)
                        .ThenInclude(uqr => uqr.Quiz) // Kullanıcının sonucu ait quiz bilgisi
                .OrderBy(q => q.Id)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Question?> GetNextQuestionAsync(int quizId, int currentQuestionId)
        {
            return await _context.Questions
                .Where(q => q.QuizId == quizId && q.Id > currentQuestionId)
                .OrderBy(q => q.Id)
                .Include(q => q.Quiz)
                    .ThenInclude(qz => qz.Category)
                .Include(q => q.Quiz)
                    .ThenInclude(qz => qz.TestType)
                .FirstOrDefaultAsync();
        }


        public async Task<Question?> GetPreviousQuestionAsync(int quizId, int currentQuestionId)
        {
            return await _context.Questions
                .Where(q => q.QuizId == quizId && q.Id < currentQuestionId)
                .OrderByDescending(q => q.Id)
                .Include(q => q.Quiz)
                    .ThenInclude(qz => qz.Category)
                .Include(q => q.Quiz)
                    .ThenInclude(qz => qz.TestType)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }


        public async Task<List<Question>> SearchQuestionsAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return new List<Question>();

            return await _context.Questions
                .Where(q => EF.Functions.Like(q.Text, $"%{searchText}%"))
                .OrderBy(q => q.Id)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Quiz?> GetByIdAsync(int id)
        {
            return await _context.Quizzes
                .Include(q => q.Category)
                .Include(q => q.TestType)
                .Include(q => q.Questions)
                    .ThenInclude(q => q.UserAnswers)
                        .ThenInclude(ua => ua.UserQuizResult)
                            .ThenInclude(uqr => uqr.User) // null'ı engeller
                .Include(q => q.Questions)
                    .ThenInclude(q => q.UserAnswers)
                        .ThenInclude(ua => ua.UserQuizResult)
                            .ThenInclude(uqr => uqr.Quiz) // null'ı engeller
                .FirstOrDefaultAsync(q => q.Id == id);
        }
        public async Task<Quiz?> GetQuizWithQuestionsAsync(int id)
        {
            return await _context.Quizzes
                .Include(q => q.Category)
                .Include(q => q.TestType)
                .Include(q => q.Questions)
                    .ThenInclude(q => q.UserAnswers)
                        .ThenInclude(ua => ua.UserQuizResult)
                            .ThenInclude(uqr => uqr.User)
                .Include(q => q.Questions)
                    .ThenInclude(q => q.UserAnswers)
                        .ThenInclude(ua => ua.UserQuizResult)
                            .ThenInclude(uqr => uqr.Quiz)
                .FirstOrDefaultAsync(q => q.Id == id);
        }


    }
}
