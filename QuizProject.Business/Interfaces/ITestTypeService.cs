using QuizProject.Business.Services;
using QuizProject.Data.Models;
using QuizProject.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizProject.Business.Interfaces
{
    public interface ITestTypeService
    {
        Task<TestType?> GetByIdAsync(int id);
        Task<List<TestType>> GetAllAsync();
        Task CreateAsync(TestType testType);
        Task UpdateAsync(TestType testType);
        Task DeleteAsync(TestType testType);
        Task RemoveRangeAsync(IEnumerable<TestType> testTypes);

        Task<List<TestType>> GetTestTypesWithQuizzesAsync();
        Task<List<TestType>> SearchTestTypesAsync(string? term);
        Task<TestType?> GetTestTypeByNameAsync(string? term);

        Task<List<TestType>> GetTestTypesByCategoryIdAsync(int categoryId);
        Task<List<TestType>> GetTestTypesByCategoryAsync(int categoryId);

    }
}
