using System;
using System.Threading.Tasks;
using Coinstantine.Common.Attributes;
using Coinstantine.Core.UIServices;
using Coinstantine.Core.ViewModels.Messages;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Auth;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross;

namespace Coinstantine.Core.Services
{
    [RegisterInterfaceAsDynamic]
    public class LogoutService : ILogoutService
    {
        private readonly IProfileProvider _profileProvider;
        private readonly INavigationService _navigationService;
        private readonly ITranslationService _translationService;
        private readonly IMessageService _messageService;
        private INotificationsRegistrationService _notificationsRegistrationService;

        public LogoutService(IProfileProvider profileProvider,
                             INavigationService navigationService,
                             ITranslationService translationService,
                             IMessageService messageService)
        {
            _profileProvider = profileProvider;
            _navigationService = navigationService;
            _translationService = translationService;
            _messageService = messageService;
        }

        private INotificationsRegistrationService NotificationsRegistrationService => _notificationsRegistrationService ?? (_notificationsRegistrationService = Mvx.Resolve<INotificationsRegistrationService>());

        public async Task SessionExpired()
        {
            await NotificationsRegistrationService.Unregister();
            var currentUser = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            if (currentUser?.LoggedIn ?? false)
            {
                await _profileProvider.Logout(false).ConfigureAwait(false);
                AlertWithAction(TranslationKeys.General.SessionExpired, _navigationService.ShowLandingPage);
            }
        }

        public async Task TooManyWrongPincodes()
        {
            await NotificationsRegistrationService.Unregister();
            await _profileProvider.Logout(true).ConfigureAwait(false);
            AlertWithAction(TranslationKeys.Pincode.TooManyWrongPincode, _navigationService.ShowLandingPage);
        }

        private string Translate(TranslationKey key)
        {
            return _translationService.Translate(key);
        }

        private string Ok => Translate(TranslationKeys.General.Ok);

        private void AlertWithAction(TranslationKey message, Action okAction)
        {
            _messageService.Alert(new MessageViewModel(Translate(message), () => { _messageService.Dismiss(); okAction(); }, null, Ok, null));
        }

        public async Task RegularLogout()
        {
            _navigationService.ShowLandingPage();
            await Task.Factory.StartNew(async () =>
            {
                await NotificationsRegistrationService.Unregister();
                await _profileProvider.Logout(false).ConfigureAwait(false);
            });
        }
    }
}