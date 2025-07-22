using System;
using System.Threading.Tasks;

namespace QuizProject.Data.Repositories.Abstract
{  //Transaction duurmlarında savechangesi en son atmamız gerekir bu nedenle (iki ya da daha fazla kayıt)//kayıtlardan birnde sıkıntı çıkarsa birisi para attığında birinin parası azalırken diğerin ki artmıyorsa işlemin geri alınması gerekir.Rollback yapılması gerekiyor.
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        ICategoryRepository Categories { get; }
        IQuizRepository Quizzes { get; }
        IQuestionRepository Questions { get; }
        IUserQuizResultRepository UserQuizResults { get; }
        ITestTypeRepository TestTypes { get; }
        IUserAnswerRepository UserAnswers { get; }
        IQuizCommentRepository QuizComments { get; }

        int Save();            // Senkron kaydetme metodu
        Task SaveAsync();      // Asenkron kaydetme metodu, int dönmüyor
    }
}
