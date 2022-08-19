using System.Collections.Generic;
using System.Threading.Tasks;
using Coinstantine.Data;

namespace Coinstantine.Domain.Interfaces
{
    public interface ISettingsService
    {
		Task<string> GetSetting(SettingKey settingKey);
		Task SetSetting(SettingKey settingKey, string value, SettingScope scope);
		Task<IDictionary<SettingKey, string>> GetAll(SettingScope scope);
    }

	public enum SettingKey
	{
		TelegramTutorial,
		TelegramProfileValidated,
		TwitterProfileValidated,
        EtherscanUrl,
        Web3Url,
        ApiEnvironment,
        EthereumNetwork,
        NotificationToken,
        UpdateBalance
    }
}
