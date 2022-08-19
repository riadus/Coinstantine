using System;
using System.Collections.Generic;
using System.Linq;
using Coinstantine.Common.Attributes;
using Coinstantine.Domain.Interfaces;

namespace Coinstantine.Domain.Mapping.Mappers
{
	[RegisterInterfaceAsDynamic]
	public class SettingKeyMapper : IMapper<string, SettingKey?>
    {
		private readonly IDictionary<SettingKey, string> _dictionary;
        public SettingKeyMapper()
        {
			_dictionary = new Dictionary<SettingKey, string>
			{
				{SettingKey.TelegramProfileValidated, "Telegram"},
				{SettingKey.TwitterProfileValidated, "Twitter"},
                {SettingKey.UpdateBalance, "Wallet"}
			};
        }

		public SettingKey? Map(string source)
		{
			if(_dictionary.Values.Contains(source))
			{
				return _dictionary.First(x => x.Value == source).Key;
			}
			return null;
		}

		public string MapBack(SettingKey? source)
		{
			if(source.HasValue && _dictionary.ContainsKey(source.Value))
			{
				return _dictionary[source.Value];
			}
			return null;
		}
	}
}
