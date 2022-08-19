using System;
using System.Reflection;
using System.Threading.Tasks;
using Coinstantine.Core.LifeTime;
using Coinstantine.Core.Services;
using Coinstantine.Data;
using Coinstantine.Data.Migrations;
using Coinstantine.Database;
using Coinstantine.Domain.Interfaces;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Coinstantine.Core
{
    public class AppStart : MvxAppStart, IMvxAppStart
    {
		private readonly ISyncService _syncService;
        private readonly IAppServices _appServices;
        private readonly ILifeCycle _lifeCycle;
        private readonly IMigrationService _migrationService;
        private readonly IProfileProvider _profileProvider;
		private readonly ITranslationService _translationProvider;
		private readonly INotificationsAnalyserService _notificationsAnalyserService;
		private readonly IDeviceInfoProvider _deviceInfoProvider;
		private readonly INavigationService _navigationService;
		private const int PermittedIdleInMinutes = 5;
		private bool _started;
        public AppStart(IMvxApplication application,
                        IMvxNavigationService mvxNavigationService,
                        IAppServices appServices,
                        ILifeCycle lifeCycle,
                        IMigrationService migrationService,
                        IProfileProvider profileProvider,
		                INotificationsAnalyserService notificationsAnalyserService,
                        IDeviceInfoProvider deviceInfoProvider) : base(application, mvxNavigationService)
        {
            _navigationService = appServices.NavigationService;
			_syncService = appServices.SyncService;
            _appServices = appServices;
            _lifeCycle = lifeCycle;
            _migrationService = migrationService;
            _profileProvider = profileProvider;
			_translationProvider = appServices.TranslationService;
			_notificationsAnalyserService = notificationsAnalyserService;
			_deviceInfoProvider = deviceInfoProvider;
            _lifeCycle.LifeCycleStateChanged += LifeCycle_LifeCycleStateChanged;
        }

        protected async Task StartApp(bool hint)
        {
            var assembly = typeof(SqliteDatabase).GetTypeInfo().Assembly;

            if (await _migrationService.NeedsMigration(assembly).ConfigureAwait(false))
            {
                Progress<MigrationProgress> progress = new Progress<MigrationProgress>(x => { });
                await _migrationService.MigrateToLatest(assembly, progress).ConfigureAwait(false);
            }
            await InitializeSystemCulture().ConfigureAwait(true);
            var profile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            if(_started && profile == null)
            {
                return;
            }
            if (UserComesBackDuringPermittedIdleTime(hint, profile))
            {
				await _notificationsAnalyserService.HandlePartToUpdateInForground().ConfigureAwait(false);
                _lifeCycle.FireOnStartForOthers();
                return;
            }

            if (profile?.LoggedIn ?? false)
            {
                _navigationService.ShowCheckPincode();
                return;
            }
            _navigationService.ShowLandingPage();
            _started = true;
        }

		async void LifeCycle_LifeCycleStateChanged(object sender, LifeCycleEventArgs e)
        {
            switch (e.LifetimeEvent)
            {
                case LifeCycleEvent.Starting:
                    await StartApp(false).ConfigureAwait(false);
                    break;
                case LifeCycleEvent.Restarting:
                    await StartApp(true).ConfigureAwait(false);
                    break;
                case LifeCycleEvent.Closing:
                    await ClosingApp().ConfigureAwait(false);
                    break;
            }
        }

		private async Task ClosingApp()
        {
            _appServices.MessageService.Dismiss();
            _appServices.OverlayService.Dismiss();
            await _appServices.MenuManager.HideMenu().ConfigureAwait(false);
            var userprofile = await _profileProvider.GetUserProfile();
            if (userprofile != null)
            {
                userprofile.LastUserSession = DateTime.Now;
                await _profileProvider.SaveUserProfile(userprofile).ConfigureAwait(false);
            }
        }

		private static bool UserComesBackDuringPermittedIdleTime(bool frombackground, UserProfile userprofile)
        {
            return IsStartedFromBackground(frombackground) && UserWasActiveDuringPermittedIdleTime(userprofile);
        }

		private static bool UserWasActiveDuringPermittedIdleTime(UserProfile userProfile)
        {
			return userProfile != null && userProfile.LastUserSession.HasValue && DateTime.Now.Subtract(userProfile.LastUserSession.Value).TotalMinutes <= PermittedIdleInMinutes;
        }

		private static bool IsStartedFromBackground(bool frombackground)
        {
            return frombackground;
        }

		private async Task InitializeSystemCulture()
        {
			var deviceCulture = _deviceInfoProvider.DeviceLanguage;
            string deviceLanguage;
            if (!deviceCulture.Contains("-"))
            {
                deviceLanguage = deviceCulture;
            }
            else
            {
                var languageAndRegion = deviceCulture.Split('-');
                deviceLanguage = languageAndRegion[0];
            }
            _translationProvider.DeviceLanguage = deviceLanguage;
			await _translationProvider.LoadTranslations(true).ConfigureAwait(false);
        }

        protected override void NavigateToFirstViewModel(object hint = null)
        {
            StartApp(false);
        }
    }
}
