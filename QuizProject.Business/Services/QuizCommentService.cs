using QuizProject.Business.Interfaces;
using QuizProject.Data.Models;
using QuizProject.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizProject.Business.Services
{
    public class QuizCommentService : IQuizCommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuizCommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<QuizComment?> GetByIdAsync(int id)
        {
            return await _unitOfWork.QuizComments.GetByIdAsync(id);
        }

        public async Task<List<QuizComment>> GetAllAsync()
        {
            return await _unitOfWork.QuizComments.GetAllAsync();
        }

        public async Task CreateAsync(QuizComment comment)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment));
            await _unitOfWork.QuizComments.CreateAsync(comment);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(QuizComment comment)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment));
            await _unitOfWork.QuizComments.UpdateAsync(comment);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(QuizComment comment)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment));
            await _unitOfWork.QuizComments.DeleteAsync(comment);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<QuizComment> comments)
        {
            if (comments == null) throw new ArgumentNullException(nameof(comments));
            await _unitOfWork.QuizComments.RemoveRangeAsync(comments);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<QuizComment>> GetCommentsByQuizIdAsync(int quizId)
        {
            return await _unitOfWork.QuizComments.GetCommentsByQuizIdAsync(quizId);
        }

        public async Task<List<QuizComment>> GetCommentsByUserIdAsync(string userId)
        {
            return await _unitOfWork.QuizComments.GetCommentsByUserIdAsync(userId);
        }

    }
}
