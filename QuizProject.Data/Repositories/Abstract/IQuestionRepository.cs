using QuizProject.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizProject.Data.Repositories.Abstract
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<List<Question>> GetQuestionsByQuizIdAsync(int quizId);

        Task<Question?> GetNextQuestionAsync(int quizId, int currentQuestionId);

        Task<Question?> GetPreviousQuestionAsync(int quizId, int currentQuestionId);

        Task<List<Question>> SearchQuestionsAsync(string searchText);
        Task<Quiz?> GetQuizWithQuestionsAsync(int id);
    }
}
