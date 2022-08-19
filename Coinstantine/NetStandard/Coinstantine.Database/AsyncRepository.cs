using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Coinstantine.Data;
using SQLite;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensionsAsync.Extensions;

namespace Coinstantine.Database
{
    public class AsyncRepository<T> : IRepository<T> where T : IEntity, new()
    {
        protected readonly SQLiteAsyncConnection _connection;
        public AsyncRepository(SQLiteAsyncConnection connection)
        {
            _connection = connection;
        }

        public async Task<bool> AnyAsync()
        {
            var item = await _connection.Table<T>().FirstOrDefaultAsync().ConfigureAwait(false);
            return item != null;
        }

        /// <summary>
        /// Get All Items
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> GetAsync()
        {
            return await GetAsync(true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get All Items
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> GetAsync(bool withChildrenRecursive)
        {
            if (withChildrenRecursive)
            {
                return await _connection.GetAllWithChildrenAsync<T>().ConfigureAwait(false);
            }
            return await _connection.Table<T>().ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Save Item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual async Task<int> SaveAsync(T item)
        {
            return await SaveAsync(item, false).ConfigureAwait(false);
        }

        public virtual async Task<int> SaveAsync(T item, bool recursive)
        {
            await _connection.InsertOrReplaceWithChildrenAsync(item, recursive).ConfigureAwait(false);
            return 1;
        }

        /// <summary>
        /// Save Item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private int SaveAsync(T item, bool recursive, SQLiteConnection connection)
        {
            connection.InsertOrReplaceWithChildren(item, recursive);
            return 1;
        }

        /// <summary>
        /// Delete Item
        /// </summary>
        /// <param name="item">The item to delete</param>
        /// <returns></returns>
        public virtual Task<int> DeleteAsync(T item)
        {
            return _connection.DeleteAsync(item);
        }

        /// <summary>
        /// Delete records that match the predicate.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        /// <param name="predicate">Predicate filter.</param>
        public virtual async Task<int> DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            var rowsAffected = 0;

            await _connection.RunInTransactionAsync((SQLiteConnection connection) =>
            {
                rowsAffected = connection.Table<T>().Delete(predicate);
            }).ConfigureAwait(false);

            return rowsAffected;
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool withChildrenRecursive)
        {
            var item = await _connection.Table<T>().Where(predicate).FirstOrDefaultAsync().ConfigureAwait(false);
            if (item != null && withChildrenRecursive)
            {
                await _connection.GetChildrenAsync(item, true).ConfigureAwait(false);
            }

            return item;
        }

        public virtual Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return GetAsync(predicate, true);
        }

        public async virtual Task<int> SaveAsync(IEnumerable<T> items)
        {
            await _connection.InsertOrReplaceAllWithChildrenAsync(items, false).ConfigureAwait(false);
            return items.Count();
        }

        public async virtual Task<int> SaveFlatListAsync(IEnumerable<T> items)
        {
            await _connection.InsertOrReplaceAllWithChildrenAsync(items, false).ConfigureAwait(false);
            return items.Count();
        }

        public async virtual Task<int> DeleteAsync(IEnumerable<T> items)
        {
            await _connection.DeleteAllAsync(items).ConfigureAwait(false);
            return items.Count();
        }

        public async virtual Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetAllAsync(predicate, true).ConfigureAwait(false);
        }

        public async virtual Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, bool withChildrenRecursive)
        {
            if (withChildrenRecursive)
            {
                return await _connection.GetAllWithChildrenAsync(predicate, withChildrenRecursive).ConfigureAwait(false);
            }
            return await _connection.Table<T>().Where(predicate).ToListAsync().ConfigureAwait(false);
        }

        public async virtual Task<int> DeleteAllAsync()
        {
            var all = await _connection.GetAllWithChildrenAsync<T>().ConfigureAwait(false);
            await _connection.DeleteAllAsync(all).ConfigureAwait(false);
            return all.Count;
        }

        public async virtual Task<IEnumerable<T>> GetRawAsync(Expression<Func<T, bool>> predicate)
        {
            var query = _connection.Table<T>().Where(predicate);
            return await query.ToListAsync().ConfigureAwait(false);
        }

        public async virtual Task<IEnumerable<T>> GetRawAsync<U>(Expression<Func<T, bool>> predicate, int limit, Expression<Func<T, U>> orderBy, bool orderDescending)
        {
            var query = _connection.Table<T>().Where(predicate);
            if (orderDescending)
            {
                query = query.OrderByDescending(orderBy);
            }
            else
            {
                query = query.OrderBy(orderBy);
            }
            return await query.Take(limit).ToListAsync().ConfigureAwait(false);
        }

        public virtual async Task<int> ReplaceAll(IEnumerable<T> newItems)
        {
            await DeleteAllAsync().ConfigureAwait(false);
            return await SaveAsync(newItems).ConfigureAwait(false);
        }

        public virtual Task<int> ReplaceAll(T item)
        {
            return ReplaceAll(new List<T> { item });
        }
    }
}