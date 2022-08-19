using System;
using System.Collections.Generic;
using System.Linq;

namespace Coinstantine.Common
{
	public static class IEnumerableExtensions
	{
        public static void Foreach<T>(this IEnumerable<T> self, Action<T> action)
		{
            foreach (T item in self)
			{
				action(item);
			}
		}

        public static IEnumerable<T> ForeachChangeValue<T>(this IEnumerable<T> self, Action<T> action)
        {
            var result = new List<T>();
            for (var i = 0; i < self.Count(); i++)
            {
                var item = self.ElementAt(i);
                action(item);
                result.Add(item);
            }
            return result;
        }

		public static IEnumerable<KeyValuePair<T1, T2>> ToKeyValuePairEnumerable<T, T1, T2>(this IEnumerable<T> items, Func<T, T1> keyFunc, Func<T, T2> valueFunc, Func<T1, bool> keyFilter = null, Func<T2, bool> valueFilter = null)
		{
			foreach (var item in items)
			{
				var key = keyFunc(item);
				var value = valueFunc(item);
				if ((keyFilter?.Invoke(key) ?? true) && (valueFilter?.Invoke(value) ?? true))
					yield return new KeyValuePair<T1, T2>(key, value);
			}
		}

		public static Dictionary<T1, T2> ToFiltredDictionary<T, T1, T2>(this IEnumerable<T> items, Func<T, T1> keyFunc, Func<T, T2> valueFunc, Func<T1, bool> keyFilter = null, Func<T2, bool> valueFilter = null)
		{
			return items.ToKeyValuePairEnumerable(keyFunc, valueFunc, keyFilter, valueFilter).ToDictionary(x => x.Key, x => x.Value);
		}
	}
}