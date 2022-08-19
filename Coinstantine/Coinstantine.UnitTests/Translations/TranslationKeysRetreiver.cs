using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Coinstantine.Common;
using Coinstantine.Domain.Interfaces.Translations;

namespace Coinstantine.UnitTests.Translations
{
    public class TranslationKeysRetreiver
    {
        private List<string> _prefixes = new List<string>();
        public IDictionary<string, IEnumerable<TranslationKey>> GetKeysFromType(Type type)
        {
            var nestedClasses = type.GetNestedTypes();
            var result = new Dictionary<string, IEnumerable<TranslationKey>>();
            foreach (var nestedClass in nestedClasses)
            {
                var keys = GetKeys(nestedClass);
                result.Add(nestedClass.ToString(), keys);
            }

            return result;
        }

        public TranslationKeysRetreiver SkipKeyStartingWith(string prefix)
        {
            _prefixes.Add(prefix);
            return this;
        }

        private IEnumerable<TranslationKey> GetKeys(Type type)
        {
            var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public).ToList();
            if (_prefixes.Any())
            {
                foreach (var prefix in _prefixes)
                {
                    fields.RemoveAll(x => x.GetValue(null)?.ToString().StartsWith(prefix, StringComparison.Ordinal) ?? true);
                }
            }
            return fields.Select(x => new TranslationKey(x.GetValue(null)?.ToString()));
        }
    }
}
