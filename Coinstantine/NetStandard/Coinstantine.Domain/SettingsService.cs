using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coinstantine.Common;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Mapping;

namespace Coinstantine.Domain
{
	[RegisterInterfaceAsDynamic]
	public class SettingsService : ISettingsService
    {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IProfileProvider _profileProvider;
		private readonly IMapper<string, SettingKey?> _settingKeyMapper;

		public SettingsService(IUnitOfWork unitOfWork,
		                       IProfileProvider profileProvider,
		                       IMapper<string, SettingKey?> settingKeyMapper)
		{
			_unitOfWork = unitOfWork;
			_profileProvider = profileProvider;
			_settingKeyMapper = settingKeyMapper;
		}
        
		public async Task<IDictionary<SettingKey, string>> GetAll(SettingScope scope)
		{
			var currentProfile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            var settings = await _unitOfWork.Settings.GetAllAsync(x => x.UserProfileEmail == currentProfile.Email && x.Scope == scope).ConfigureAwait(false);
			var keyValuePairs = ToKeyValuePairs(settings);
			return keyValuePairs.ToDictionary(x => x.Key, x => x.Value);
		}

		private IEnumerable<KeyValuePair<SettingKey, string>> ToKeyValuePairs(IEnumerable<Setting> settings)
		{
            foreach (var setting in settings)
            {
				var key = setting.Key.ToEnum<SettingKey>();
                yield return new KeyValuePair<SettingKey, string>(key, setting.Value);
            }
		}

		public async Task<string> GetSetting(SettingKey settingKey)
		{
			var currentProfile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            if(currentProfile == null)
            {
                return null;
            }
			var key = settingKey.ToString();
			var settings = await _unitOfWork.Settings.GetAllAsync(x => x.Key == key && x.UserProfileEmail == currentProfile.Email).ConfigureAwait(false);
			return settings?.FirstOrDefault()?.Value;
		}

		public async Task SetSetting(SettingKey settingKey, string value, SettingScope scope)
		{
			var setting = new Setting
			{
				Key = settingKey.ToString(),
				Value = value,
                Scope = scope
			};

			var currentProfile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            setting.UserProfileEmail = currentProfile.Email;
			var key = settingKey.ToString();
            
			var settings = await _unitOfWork.Settings.GetAllAsync(x => x.Key == key && x.UserProfileEmail == currentProfile.Email).ConfigureAwait(false);
            if(settings.Any() && settings.FirstOrDefault() != null)
			{
				setting.Id = settings.First().Id;
				setting.UserProfileEmail = settings.First().UserProfileEmail;
			}

			await _unitOfWork.Settings.SaveAsync(setting).ConfigureAwait(false);
		}
	}
}
