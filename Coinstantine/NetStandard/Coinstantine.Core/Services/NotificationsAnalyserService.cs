using System.Collections.Generic;
using System.Threading.Tasks;
using Coinstantine.Common.Attributes;
using Coinstantine.Core.UIServices;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Translations;
using Coinstantine.Domain.Mapping;
using Coinstantine.Domain.Messages;
using MvvmCross.Plugin.Messenger;
using Plugin.Connectivity.Abstractions;

namespace Coinstantine.Core.Services
{
	[RegisterInterfaceAsLazySingleton]
	public class NotificationsAnalyserService : INotificationsAnalyserService
	{
		private readonly IProfileProvider _profileProvider;
		private readonly IConnectivity _connectivity;
        private readonly ISyncService _syncService;
        private readonly ISettingsService _settingsService;
        private readonly IMessageService _messageService;
        private readonly INavigationService _navigationService;
        private readonly IMvxMessenger _mvxMessenger;
        private readonly ITranslationService _translationService;
        private readonly IMapper<string, SettingKey?> _settingKeyMapper;

		public NotificationsAnalyserService(IProfileProvider profileProvider,
                                            IConnectivity connectivity,
		                                    ISyncService syncService,
		                                    ISettingsService settingsService,
                                            IMessageService messageService,
                                            INavigationService navigationService,
                                            IMvxMessenger mvxMessenger,
                                            ITranslationService translationService,
		                                    IMapper<string, SettingKey?> settingKeyMapper)
		{
			_profileProvider = profileProvider;
			_connectivity = connectivity;
            _syncService = syncService;
            _settingsService = settingsService;
            _messageService = messageService;
            _navigationService = navigationService;
            _mvxMessenger = mvxMessenger;
            _translationService = translationService;
            _settingKeyMapper = settingKeyMapper;
		}

		public async Task HandlePartToUpdate(string partToUpdate, bool isAppInForground)
		{
			var nullableKey = _settingKeyMapper.Map(partToUpdate);
			if(nullableKey == null)
			{
				return;
			}
			await HandlePartToUpdate(nullableKey.Value, isAppInForground).ConfigureAwait(false);
		}

		public async Task HandlePartToUpdate(SettingKey key, bool isAppInForground)
		{
            if (isAppInForground)
			{
				var processed = await _settingsService.GetSetting(key).ConfigureAwait(false);
				if (processed != null && processed == NotificationProcess.Processed.ToString())
				{
					return;
				}
                if(key == SettingKey.TelegramProfileValidated)
                {
                    var userProfile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
                    if(userProfile.TelegramProfile?.Validated ?? false)
                    {
                        await _settingsService.SetSetting(key, NotificationProcess.Processed.ToString(), SettingScope.ReceivedNotifications).ConfigureAwait(false);
                        return;
                    }
                }
                if(key == SettingKey.UpdateBalance)
                {
                    _mvxMessenger.Publish(new BuyingDoneMessage(this));
                    var sucessMessage = string.Format(_translationService.Translate(TranslationKeys.Buy.AlertBuySuccess), "");
                    _messageService.Alert(sucessMessage);
                    await _settingsService.SetSetting(key, null, SettingScope.ReceivedNotifications).ConfigureAwait(false);
                    return;
                }
                if (_connectivity.IsConnected)
				{
                    await _syncService.CheckOnlineProfileAndUnpdateIfNeeded().ConfigureAwait(false);
					await _settingsService.SetSetting(key, NotificationProcess.Processed.ToString(), SettingScope.ReceivedNotifications).ConfigureAwait(false);
                    if (key == SettingKey.TelegramProfileValidated)
                    {
                        var userProfile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
                        if (userProfile.TelegramProfile?.Validated ?? false)
                        {
                            _navigationService.ShowTelegramPage(userProfile);
                            return;
                        }
                    }
					return;
				}
			}
			await _settingsService.SetSetting(key, NotificationProcess.ToBeProcessed.ToString(), SettingScope.ReceivedNotifications).ConfigureAwait(false);
		}

        public async Task HandlePartToUpdateInForground()
		{
			var settings = await _settingsService.GetAll(SettingScope.ReceivedNotifications).ConfigureAwait(false);
			foreach (var setting in settings)
			{
				if (setting.Value == null || setting.Value == NotificationProcess.Processed.ToString())
				{
					return;
				}
				await HandlePartToUpdate(setting.Key, true).ConfigureAwait(false);
			}
		}

        private enum NotificationProcess
		{
			Processed,
			ToBeProcessed
		}
	}
}
