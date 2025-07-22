using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace QuizProject.Data.Repositories.Abstract
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id, bool tracking = true);
        Task<T?> GetOneAsync(
            Expression<Func<T, bool>>? filter = null,
            bool tracking = true,
            params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            bool tracking = true,
            params Expression<Func<T, object>>[] includes);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entities);
    }

}
