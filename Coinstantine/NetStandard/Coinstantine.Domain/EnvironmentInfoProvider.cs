using System.Collections.Generic;
using System.Threading.Tasks;
using Coinstantine.Common;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;

namespace Coinstantine.Domain
{
    [RegisterInterfaceAsLazySingleton]
    public class EnvironmentInfoProvider : IEnvironmentInfoProvider
    {
        private readonly ISettingsService _settingsService;
        private readonly IBackendService _backendService;
        private IDictionary<SettingKey, string> _valueDictionary;

        public EnvironmentInfoProvider(IBackendService backendService,
                                       ISettingsService settingsService)
        {
            _backendService = backendService;
            _settingsService = settingsService;
            _valueDictionary = new Dictionary<SettingKey, string>
            {
                {SettingKey.ApiEnvironment, string.Empty},
                {SettingKey.EthereumNetwork, string.Empty},
                {SettingKey.EtherscanUrl, string.Empty},
                {SettingKey.Web3Url, string.Empty}
            };
        }

        public async Task SetUrlsFromBackend()
        {
            var env = await _backendService.GetEnvironmentInfo().ConfigureAwait(false);
            if(env == null)
            {
                return;
            }

            _valueDictionary = new Dictionary<SettingKey, string>
            {
                {SettingKey.ApiEnvironment, string.Empty},
                {SettingKey.EthereumNetwork, string.Empty},
                {SettingKey.EtherscanUrl, string.Empty},
                {SettingKey.Web3Url, string.Empty}
            };

            await _settingsService.SetSetting(SettingKey.EthereumNetwork, env.EthereumNetwork.ToString(), SettingScope.General).ConfigureAwait(false);
            await _settingsService.SetSetting(SettingKey.ApiEnvironment, env.ApiEnvironment.ToString(), SettingScope.General).ConfigureAwait(false);
            await _settingsService.SetSetting(SettingKey.EtherscanUrl, env.EtherscanUrl, SettingScope.General).ConfigureAwait(false);
            await _settingsService.SetSetting(SettingKey.Web3Url, env.Web3Url, SettingScope.General).ConfigureAwait(false);
        }

        public Task<string> GetWeb3Url()
        {
            return GetValue(SettingKey.Web3Url);
        }

        public Task<string> GetEtherscanUrl()
        {
            return GetValue(SettingKey.EtherscanUrl);
        }

        public async Task<string> GetValue(SettingKey settingKey)
        {
            var (Found, Value) = await TryGetValueFromSettings(settingKey).ConfigureAwait(false);
            if(Found)
            {
                return Value;
            }
            await SetUrlsFromBackend().ConfigureAwait(false);
            return await GetValue(settingKey).ConfigureAwait(false);
        }

        private async Task<(bool Found, string Value)> TryGetValueFromSettings(SettingKey settingKey)
        {
            if (_valueDictionary.ContainsKey(settingKey))
            {
                if (_valueDictionary[settingKey].IsNotNull())
                {
                    return (true, _valueDictionary[settingKey]);
                }

                var setting = await _settingsService.GetSetting(settingKey).ConfigureAwait(false);
                if (setting.IsNotNull())
                {
                    _valueDictionary[settingKey] = setting;
                    return (true, setting);
                }
                return (false, string.Empty);
            }
            throw new System.InvalidOperationException($"{settingKey} not included in environment info provider");
        }
    }
}
