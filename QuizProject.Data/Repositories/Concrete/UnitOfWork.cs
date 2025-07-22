using QuizProject.Data.Repositories.Abstract;
using QuizProject.Data.Models;
using System;
using System.Threading.Tasks;

namespace QuizProject.Data.Repositories.Concrete
{
    public class UnitOfWork : IUnitOfWork, IDisposable, IAsyncDisposable
    {
        private readonly AppDbContext _context;

        public ICategoryRepository Categories { get; private set; }
        public IQuizRepository Quizzes { get; private set; }
        public IQuestionRepository Questions { get; private set; }
        public IUserQuizResultRepository UserQuizResults { get; private set; }
        public ITestTypeRepository TestTypes { get; private set; }
        public IUserAnswerRepository UserAnswers { get; private set; }
        public IQuizCommentRepository QuizComments { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            Categories = new CategoryRepository(_context);
            Quizzes = new QuizRepository(_context);
            Questions = new QuestionRepository(_context);
            UserQuizResults = new UserQuizResultRepository(_context);
            TestTypes = new TestTypeRepository(_context);
            UserAnswers = new UserAnswerRepository(_context);
            QuizComments = new QuizCommentRepository(_context);
        }

        public int Save() => _context.SaveChanges();

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();

        public ValueTask DisposeAsync() => _context.DisposeAsync();
    }


}
