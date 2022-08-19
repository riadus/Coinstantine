using System.Collections.ObjectModel;
using Coinstantine.Common;
using Coinstantine.Core.Services;
using Coinstantine.Core.ViewModels.Settings;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.Commands;

namespace Coinstantine.Core.ViewModels
{
    public class ValidateProfileViewModel : BaseViewModel
    {
		public ValidateProfileViewModel(IAppServices appServices,
                                        TwitterItemProfileViewModel twitterItemProfileViewModel,
                                        TelegramItemProfileViewModel telegramItemProfileViewModel,
                                        BitcoinTalkItemProfileViewModel bitcoinTalkItemProfileViewModel) : base(appServices)
        {
            Title = TranslationKeys.MainMenu.ValidateProfile;
            TitleIcon = "user";
            UserProfileItems = new ObservableCollection<SettingItemViewModel>
            {
                twitterItemProfileViewModel,
                telegramItemProfileViewModel,
                bitcoinTalkItemProfileViewModel
            };

            SelectedThirdPartyItemCommand = new MvxCommand<SettingItemViewModel>(item => SelectedThirdPartyItem = item);
        }

        public ObservableCollection<SettingItemViewModel> UserProfileItems { get; }

        private SettingItemViewModel _selectedThirdPartyItem;

        public IMvxCommand<SettingItemViewModel> SelectedThirdPartyItemCommand { get; }

        public SettingItemViewModel SelectedThirdPartyItem
        {
            get
            {
                return _selectedThirdPartyItem;
            }

            set
            {
                _selectedThirdPartyItem = value;
                if (_selectedThirdPartyItem?.SelectedCommand != null)
                {
                    _selectedThirdPartyItem.SelectedCommand.Execute();
                }
                _selectedThirdPartyItem = null;
                RaisePropertyChanged();
            }
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            TrackPage("Validate profile menu page");
            UserProfileItems.Foreach(x => x.RaiseAllPropertiesChanged());
        }
    }
}
