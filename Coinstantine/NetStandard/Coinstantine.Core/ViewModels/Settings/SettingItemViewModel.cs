using System;
using System.Threading.Tasks;
using Coinstantine.Core.Services;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;
using MvvmCross.Commands;

namespace Coinstantine.Core.ViewModels.Settings
{
    public class SettingItemViewModel : BaseViewModel
    {
        private readonly IProfileProvider _profileProvider;

        protected UserProfile UserProfile { get; private set; }

        public SettingItemViewModel(IAppServices appServices,
                                    IProfileProvider profileProvider) : base(appServices)
        {
            UserProfile = profileProvider.GetUserProfile().Result;
            _profileProvider = profileProvider;
        }

        public bool HasValidation { get; set; }
        public Func<bool> IsValidatedFunc { get; set; }
        public bool Validated => HasValidation && IsValidatedFunc();
        public bool NotValidated => HasValidation && !IsValidatedFunc();

        public string IconTitle { get; set; }
        public string SettingTitle => GetTitle();
        public IMvxCommand SelectedCommand { get; set; }

        protected async Task UpdateUserProfile()
        {
            UserProfile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            TrackPage("Settings menu page");
        }
    }
}
