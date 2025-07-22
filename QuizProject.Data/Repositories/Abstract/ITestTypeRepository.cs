using QuizProject.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizProject.Data.Repositories.Abstract
{
    public interface ITestTypeRepository : IRepository<TestType>
    {
        Task<List<TestType>> GetTestTypesWithQuizzesAsync(); // İçinde quiz bulunan test türlerini getirir

        Task<List<TestType>> SearchTestTypesAsync(string? term);
        Task<TestType?> GetTestTypeByNameAsync(string? term);
        Task<List<TestType>> GetTestTypesByCategoryAsync(int categoryId);

    }
}
