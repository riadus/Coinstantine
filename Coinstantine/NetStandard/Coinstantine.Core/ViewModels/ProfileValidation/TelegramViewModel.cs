using System;
using System.Threading.Tasks;
using Coinstantine.Common;
using Coinstantine.Core.LifeTime;
using Coinstantine.Core.Services;
using Coinstantine.Core.UIServices;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.ViewModels.ProfileValidation
{
    public class TelegramViewModel : BaseViewModel
    {
        private readonly ITutorialService _tutorialService;
        private readonly ISettingsService _settingsService;
        private readonly IUserService _userService;
        private readonly ILifeCycle _lifeCycle;
        private readonly IProfileProvider _profileProvider;
        private readonly IAppOpener _appOpener;
        private readonly ISyncService _syncService;

        public TelegramViewModel(IAppServices appServices,
                                 ITutorialService tutorialService,
                                 ISettingsService settingsService,
                                 IUserService userService,
                                 ILifeCycle lifeCycle,
                                 IProfileProvider profileProvider,
                                 IAppOpener appOpener,
                                 ISyncService syncService) : base(appServices)
        {
            TutorialButtonCommand = new MvxAsyncCommand(StartTutorial);
            StartConversationCommand = new MvxAsyncCommand(StartConversation);
            _tutorialService = tutorialService;
            _settingsService = settingsService;
            _userService = userService;
            _lifeCycle = lifeCycle;
            _profileProvider = profileProvider;
            _appOpener = appOpener;
            _syncService = syncService;
            _lifeCycle.LifeCycleChangedForOther += _lifeCycle_LifeCycleChangedForOther;

            Title = TranslationKeys.General.TelegramAccount;
            TitleIcon = "telegram-plane";
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            TrackPage("Telegram page");
        }

        public override async void Prepare()
        {
            base.Prepare();
            var user = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            Username = user.TelegramProfile?.Username ?? user.TelegramProfile?.InAppUsername;
            RaisePropertyChanged(nameof(Username));
        }

        public string Username { get; set; }

        public IMvxCommand TutorialButtonCommand { get; }
        private async Task StartTutorial()
        {
            var setting = await _settingsService.GetSetting(SettingKey.TelegramTutorial).ConfigureAwait(true);
            await _settingsService.SetSetting(SettingKey.TelegramTutorial, "true", SettingScope.FollowedTutorial).ConfigureAwait(true);
            _tutorialService.StartTelegramTutorial(new Action(StartTelegram));
        }

        public IMvxCommand StartConversationCommand { get; }
        private async Task StartConversation()
        {
            if (Username.IsNullOrEmpty())
            {
                Alert(TranslationKeys.Telegram.UsernameNeededError);
                return;
            }
            var setting = await _settingsService.GetSetting(SettingKey.TelegramTutorial).ConfigureAwait(true);
            if (setting == null || setting != "true")
            {
                await StartTutorial();
            }
            else
            {
                StartTelegram();
            }
        }

        private void StartTelegram()
        {
            if (!AppServices.Connectivity.IsConnected)
            {
                ShowNoConnectionMessage();
                return;
            }
            if (Username.IsNotNull())
            {
                Username = Username.Sanitize();
                AlertWithAction(TranslationKeys.Telegram.Alert, async () =>
                {
                    await _userService.StartTelegramConversation(Username);
                    _needToUpdate = true;
                    if (_appOpener.CanOpenTelegram)
                    {
                        _appOpener.OpenTelegram();
                    }
                    else
                    {
                        _appOpener.OpenTelegramInBrowser();
                    }
                });
            }
        }

        bool _needToUpdate;

        public string TutorialButtonText => Translate(TranslationKeys.Telegram.WatchTutorial);

        async void _lifeCycle_LifeCycleChangedForOther(object sender, LifeCycleEventArgs e)
        {
            if (_needToUpdate)
            {
                Wait(TranslationKeys.Telegram.LoadingTelegram);
                var (profile, success) = await _userService.GetTelegramProfile(Username).ConfigureAwait(false);
                DismissWaitMessage();
                if (success)
                {
                    Close(this);
                    AppNavigationService.ShowTelegramPage(profile);
                }
                else
                {
                    Alert(TranslationKeys.Telegram.CouldNotCheckUser);
                }
                _needToUpdate = false;
            }
        }

        public string StartConversationButtonText => Translate(TranslationKeys.Telegram.StartConversation);
        public string UsernamePlaceholder => Translate(TranslationKeys.Profile.Telegram);
        public string ExplanationText => Translate(TranslationKeys.Telegram.ExplanationText);

        public override void ViewDestroy(bool viewFinishing = true)
        {
            _lifeCycle.LifeCycleChangedForOther -= _lifeCycle_LifeCycleChangedForOther;
            base.ViewDestroy(viewFinishing);
        }
    }
}
