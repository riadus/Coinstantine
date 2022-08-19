using System.Threading.Tasks;
using Coinstantine.Core.Services;
using Coinstantine.Core.ViewModels.Settings;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.ViewModels
{
    public class TelegramItemProfileViewModel : SettingItemViewModel
    {
        public TelegramItemProfileViewModel(IAppServices appServices,
                                           IProfileProvider profileProvider) : base(appServices, profileProvider)
        {
            IconTitle = "telegram-plane";
            Title = TranslationKeys.Settings.Telegram;
            HasValidation = true;
            IsValidatedFunc = IsValidatedFunc = () =>
            {
                UpdateUserProfile().ConfigureAwait(false);
                return UserProfile?.TelegramProfile?.Validated ?? false;
            };
            SelectedCommand = new MvxCommand(ShowTelegramPage);
        }

        private void ShowTelegramPage()
        {
            if (Validated)
            {
                AppNavigationService.ShowTelegramPage(UserProfile);
            }
            else
            {
                AppNavigationService.ShowTelegramPage();
            }
        }
    }
}
