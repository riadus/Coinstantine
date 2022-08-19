using System;
using System.Threading.Tasks;
using Coinstantine.Core.Services;
using Coinstantine.Core.ViewModels.Settings;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.ViewModels
{
    public class TwitterItemProfileViewModel : SettingItemViewModel
    {
		public TwitterItemProfileViewModel(IAppServices appServices,
                                           IProfileProvider profileProvider) : base(appServices, profileProvider)
        {
            IconTitle = "twitter";
            Title = TranslationKeys.Settings.Twitter;
            HasValidation = true;
            IsValidatedFunc = () =>
            {
                UpdateUserProfile().ConfigureAwait(false);
                return UserProfile.TwitterProfile?.Validated ?? false;
            };
            SelectedCommand = new MvxCommand(ShowTwitterPage);
        }

        private void ShowTwitterPage()
        {
            if (Validated)
            {
                AppNavigationService.ShowTwitterPage(UserProfile);
            }
            else
            {
                AppNavigationService.ShowTwitterPage();
            }
        }
    }
}
