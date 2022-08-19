using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Coinstantine.Data;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;

namespace Coinstantine.Database
{
    public class AsyncCachedRepository<T> : AsyncRepository<T> where T : IEntity, new()
    {
        private IEnumerable<T> _cachedList;
        private bool _forceReloadCache;
        public AsyncCachedRepository(SQLiteAsyncConnection connection) : base(connection)
        {
            _cachedList = new List<T>();
        }

        protected async Task LoadCacheIfNeeded()
        {
            if (!_cachedList.Any() || _forceReloadCache)
            {
                _cachedList = await base.GetAsync(true).ConfigureAwait(false);
                _forceReloadCache = false;
            }
        }

        public override async Task<IEnumerable<T>> GetAsync(bool withChildrenRecursive)
        {
            await LoadCacheIfNeeded().ConfigureAwait(false);
            return _cachedList;
        }

        public override async Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool withChildrenRecursive)
        {
            await LoadCacheIfNeeded().ConfigureAwait(false);
            return _cachedList.FirstOrDefault(predicate.Compile());
        }

        public override async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, bool withChildrenRecursive)
        {
            await LoadCacheIfNeeded().ConfigureAwait(false);
            return _cachedList.Where(predicate.Compile());
        }

        public override async Task<int> ReplaceAll(IEnumerable<T> newItems)
        {
            _cachedList = newItems;
            await Task.Factory.StartNew(async () =>
            {
                await DeleteAllAsync();
                await _connection.InsertOrReplaceAllWithChildrenAsync(newItems, false).ConfigureAwait(false);
            });
            return 1;
        }

        public override Task<int> ReplaceAll(T item)
        {
            return ReplaceAll(new List<T> { item });
        }

        public override Task<int> SaveAsync(T item)
        {
            _forceReloadCache = true;
            return base.SaveAsync(item);
        }

        public override Task<int> SaveAsync(IEnumerable<T> items)
        {
            _forceReloadCache = true;
            return base.SaveAsync(items);
        }

        public override Task<int> SaveAsync(T item, bool recursive)
        {
            _forceReloadCache = true;
            return base.SaveAsync(item, recursive);
        }

        public override Task<int> SaveFlatListAsync(IEnumerable<T> items)
        {
            _forceReloadCache = true;
            return base.SaveFlatListAsync(items);
        }
    }
}