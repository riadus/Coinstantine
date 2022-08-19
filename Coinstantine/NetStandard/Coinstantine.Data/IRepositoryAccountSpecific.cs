using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Coinstantine.Data
{
    public interface IRepositoryAccountSpecific<T> where T : IEntity, new()
    {
        Task<bool> AnyAsync();
        Task<int> DeleteAsync(T item);
        Task<int> DeleteAsync(IEnumerable<T> items);
        Task<int> DeleteAsync(Expression<Func<T, bool>> predicate);
        Task<int> DeleteAllAsync();

        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool withChildrenRecursive);
        Task<IEnumerable<T>> GetAsync();
        Task<IEnumerable<T>> GetAsync(bool withChildrenRecursive);
        Task<int> SaveAsync(T item);
        Task<int> SaveAsync(T item, bool recursive);
        Task<int> SaveAsync(IEnumerable<T> items);
        Task<int> SaveFlatListAsync(IEnumerable<T> items);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, bool withChildrenRecursive);

        Task<IEnumerable<T>> GetRawAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetRawAsync<U>(Expression<Func<T, bool>> predicate, int limit, Expression<Func<T, U>> orderBy, bool orderDescending);
    }
}
