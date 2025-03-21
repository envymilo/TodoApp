using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Infrastructure.Repositories.Generic
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetPagedAsync(int page, int pageSize);
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<bool> UpdateAsync(Guid id, T entity);
        Task<bool> UpdateRange(IEnumerable<T> entities);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteRangeAsync(IEnumerable<Guid> ids);

    }
}
