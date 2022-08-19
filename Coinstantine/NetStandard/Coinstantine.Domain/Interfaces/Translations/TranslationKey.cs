using System.Collections.Generic;

namespace Coinstantine.Domain.Interfaces.Translations
{
    public class TranslationKey
    {
        public string Key { get; }
        public TranslationKey(string key)
        {
            Key = key;
        }

        public static implicit operator TranslationKey(string value)
        {
            return new TranslationKey(value);
        }

		public override string ToString()
		{
			return Key;
		}

		public override bool Equals(object obj)
		{
			var translationKey = obj as TranslationKey;
			if (obj == null)
			{
				return false;
			}
			return Key.Equals(translationKey.Key);
		}

		public override int GetHashCode()
		{
			return 990326508 + EqualityComparer<string>.Default.GetHashCode(Key);
		}
	}
}
