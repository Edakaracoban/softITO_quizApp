using QuizProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizProject.Business.Interfaces
{
    public interface IQuestionService
    {
        Task<Question?> GetByIdAsync(int id);
        Task<List<Question>> GetAllAsync();
        Task CreateAsync(Question question);
        Task UpdateAsync(Question question);
        Task DeleteAsync(Question question);
        Task RemoveRangeAsync(IEnumerable<Question> questions);

        // Özel metotlar
        Task<List<Question>> GetQuestionsByQuizIdAsync(int quizId);
        Task<Question?> GetNextQuestionAsync(int quizId, int currentQuestionId);
        Task<Question?> GetPreviousQuestionAsync(int quizId, int currentQuestionId);
        Task<List<Question>> SearchQuestionsAsync(string searchText);
        Task<Quiz?> GetQuizWithQuestionsAsync(int id);

    }
}
