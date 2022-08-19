using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Coinstantine.Data;

namespace Coinstantine.UnitTests
{
    public class MockRepository<T> : IRepository<T> where T : IEntity, new()
    {
        protected readonly IEnumerable<T> _items;

        int _lastId = 0;

        public List<T> InsertedItems { get; private set; } = new List<T>();
        public List<T> UpdatedItems { get; private set; } = new List<T>();
        public List<T> DeletedItems { get; private set; } = new List<T>();

        public MockRepository()
        {
            _items = new List<T>();
        }

        public Task<bool> AnyAsync()
        {
            return Task.FromResult(_items.Any());
        }

        public Task<int> DeleteAllAsync()
        {
            foreach (var item in InsertedItems)
            {
                DeletedItems.Add(item);
            }
            return Task.FromResult(DeletedItems.Count);
        }

        public Task<int> DeleteAsync(T item)
        {
            DeletedItems.Add(item);
            return Task.FromResult(1);
        }

        public Task<int> DeleteAsync(IEnumerable<T> items)
        {
            DeletedItems.AddRange(items);
            return Task.FromResult(items.Count());
        }

        public Task<int> DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            var items = InsertedItems.Where(predicate.Compile()).ToList();
            DeletedItems.AddRange(items);
            return Task.FromResult(items.Count);
        }

        public Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return Task.FromResult(_items.Where(predicate.Compile()));
        }

        public Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, bool withChildrenRecursive)
        {
            return GetAllAsync(predicate);
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            var item = _items.AsQueryable().FirstOrDefault(predicate);

            if (item == null && InsertedItems.Any())
            {
                item = InsertedItems.AsQueryable().FirstOrDefault(predicate);
            }
            return Task.FromResult(item);
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool withChildrenRecursive)
        {
            return GetAsync(predicate);
        }

        public Task<IEnumerable<T>> GetAsync()
        {
            return Task.FromResult(_items);
        }

        public Task<IEnumerable<T>> GetAsync(bool withChildrenRecursive)
        {
            return GetAsync();
        }

        public Task<IEnumerable<T>> GetRawAsync(Expression<Func<T, bool>> predicate)
        {
            return GetAllAsync(predicate, true);
        }

        public async Task<IEnumerable<T>> GetRawAsync<U>(Expression<Func<T, bool>> predicate, int limit, Expression<Func<T, U>> orderBy, bool orderDescending)
        {
            var itemsToReturn = (await GetAllAsync(predicate, true).ConfigureAwait(false));
            if (orderDescending) itemsToReturn = itemsToReturn.OrderByDescending(orderBy.Compile());
            else itemsToReturn = itemsToReturn.OrderBy(orderBy.Compile());
            return itemsToReturn.Take(limit);
        }

        public Task<int> SaveAsync(T item)
        {
            return SaveAsync(item, false);
        }

        public Task<int> SaveAsync(T item, bool recursive)
        {
            if (item.Id > 0)
            {
                UpdatedItems.Add(item);
            }
            else
            {
                item.Id = _items.Count() + 1;
                InsertedItems.Add(item);
            }
            return Task.FromResult(1);
        }

        public async Task<int> SaveAsync(IEnumerable<T> items)
        {
            var c = 0;
            foreach (var item in items)
            {
                c += await SaveAsync(item, true);
            }
            return c;
        }

        public async Task<int> SaveFlatListAsync(IEnumerable<T> items)
        {
            return await SaveAsync(items);
        }

        public Task<int> ReplaceAll(IEnumerable<T> newItems)
        {
            return SaveAsync(newItems);
        }

        public Task<int> ReplaceAll(T item)
        {
            return SaveAsync(item);
        }
    }
}
