using Coinstantine.Core.Services;
using Coinstantine.Core.ViewModels.Settings;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.Commands;

namespace Coinstantine.Core.ViewModels
{
    public class BitcoinTalkItemProfileViewModel : SettingItemViewModel
    {
		public BitcoinTalkItemProfileViewModel(IAppServices appServices, IProfileProvider profileProvider) : base(appServices, profileProvider)
        {
            IconTitle = "bitcoin";
            Title = TranslationKeys.Settings.BitcoinTalk;
            HasValidation = true;
            IsValidatedFunc = IsValidatedFunc = () =>
            {
                UpdateUserProfile().ConfigureAwait(false);
                return UserProfile?.BitcoinTalkProfile?.Validated ?? false;
            };
            SelectedCommand = new MvxCommand(ShowBitcoinTalkPage);
        }

        private void ShowBitcoinTalkPage()
        {
            if(Validated)
            {
                AppNavigationService.ShowBitcoinTalkPage(UserProfile);
            }
            else
            {
                AppNavigationService.ShowBitcoinTalkPage();
            }
        }
    }
}
