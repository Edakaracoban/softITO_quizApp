using Microsoft.EntityFrameworkCore;
using QuizProject.Data;
using QuizProject.Data.Models;
using QuizProject.Data.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizProject.Data.Repositories.Concrete
{
    public class QuizCommentRepository : EfCoreGenericRepository<QuizComment, AppDbContext>, IQuizCommentRepository
    {
        public QuizCommentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<QuizComment>> GetCommentsByQuizIdAsync(int quizId)
        {
            return await _context.QuizComments
                .Where(qc => qc.QuizId == quizId)
                .Include(qc => qc.User)
                .OrderByDescending(qc => qc.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<QuizComment>> GetCommentsByUserIdAsync(string userId)
        {
            return await _context.QuizComments
                .Where(qc => qc.UserId == userId)
                .Include(qc => qc.Quiz)
                .OrderByDescending(qc => qc.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
