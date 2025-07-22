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
    public class TestTypeService : ITestTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TestTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<TestType?> GetByIdAsync(int id)
        {
            return await _unitOfWork.TestTypes.GetByIdAsync(id);
        }

        public async Task<List<TestType>> GetAllAsync()
        {
            return await _unitOfWork.TestTypes.GetAllAsync();
        }

        public async Task CreateAsync(TestType testType)
        {
            if (testType == null) throw new ArgumentNullException(nameof(testType));
            await _unitOfWork.TestTypes.CreateAsync(testType);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(TestType testType)
        {
            if (testType == null) throw new ArgumentNullException(nameof(testType));
            await _unitOfWork.TestTypes.UpdateAsync(testType);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(TestType testType)
        {
            if (testType == null) throw new ArgumentNullException(nameof(testType));
            await _unitOfWork.TestTypes.DeleteAsync(testType);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<TestType> testTypes)
        {
            if (testTypes == null) throw new ArgumentNullException(nameof(testTypes));
            await _unitOfWork.TestTypes.RemoveRangeAsync(testTypes);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<TestType>> GetTestTypesWithQuizzesAsync()
        {
            return await _unitOfWork.TestTypes.GetTestTypesWithQuizzesAsync();
        }
        public async Task<TestType?> GetTestTypeByNameAsync(string? term)
        {
            return await _unitOfWork.TestTypes.GetTestTypeByNameAsync(term);
        }
        public async Task<List<TestType>> SearchTestTypesAsync(string? term)
        {
            return await _unitOfWork.TestTypes.SearchTestTypesAsync(term);
        }
        public async Task<List<TestType>> GetTestTypesByCategoryIdAsync(int categoryId)
        {
            return await _unitOfWork.TestTypes.GetAllAsync(t => t.Quizzes.Any(q => q.CategoryId == categoryId));
        }

        public Task<List<TestType>> GetTestTypesByCategoryAsync(int categoryId)
        {
            throw new NotImplementedException();
        }
    }
}
