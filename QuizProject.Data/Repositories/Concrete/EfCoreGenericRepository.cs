using Microsoft.EntityFrameworkCore;
using QuizProject.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace QuizProject.Data.Repositories.Concrete
{
    public class EfCoreGenericRepository<T, TContext> : IRepository<T>
        where T : class
        where TContext : DbContext
    {
        protected readonly TContext _context;

        public EfCoreGenericRepository(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<T?> GetByIdAsync(int id, bool tracking = true)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (!tracking && entity != null)
                _context.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public async Task<T?> GetOneAsync(
            Expression<Func<T, bool>>? filter = null,
            bool tracking = true,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (!tracking)
                query = query.AsNoTracking();

            foreach (var include in includes)
                query = query.Include(include);

            return filter == null
                ? await query.FirstOrDefaultAsync()
                : await query.FirstOrDefaultAsync(filter);
        }

        public async Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            bool tracking = true,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (!tracking)
                query = query.AsNoTracking();

            foreach (var include in includes)
                query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

            return await query.ToListAsync();
        }

        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            return Task.CompletedTask;
        }
    }
}
