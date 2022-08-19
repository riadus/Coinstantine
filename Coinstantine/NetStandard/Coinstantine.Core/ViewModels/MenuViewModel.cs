using System;
using System.Collections.ObjectModel;
using Coinstantine.Core.Services;
using Coinstantine.Domain.Interfaces.Auth;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.Commands;

namespace Coinstantine.Core.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        private readonly ILogoutService _logoutService;

        public MenuViewModel(IAppServices appServices,
                             ILogoutService logoutService) : base(appServices)
        {
            _logoutService = logoutService;
            ValidateProfilesCommand = new MvxCommand(ValidateProfiles);
            LogoutCommand = new MvxCommand(Logout);
            SettingsCommand = new MvxCommand(Settings);
            WalletCommand = new MvxCommand(Wallet);
        }


        public override void Start()
        {
            base.Start();
            BuildList();
        }

        private void BuildList()
		{
            Items = new ObservableCollection<MenuItemViewModel>
            {
                new MenuItemViewModel(AppServices)
                {
                    Text = Translate(TranslationKeys.MainMenu.Wallet),
                    IconText = "wallet",
                    SelectionCommand = WalletCommand
                },
                new MenuItemViewModel(AppServices)
                {
                    Text = Translate(TranslationKeys.MainMenu.Airdrops),
                    IconText = "airdrop",
                    IsEnabled = false
                },
                new MenuItemViewModel(AppServices)
                {
                    Text = Translate(TranslationKeys.General.Logout),
                    IconText = "sign-out-alt",
                    SelectionCommand = LogoutCommand
                },
                new MenuItemViewModel(AppServices)
                {
                    Text = Translate(TranslationKeys.MainMenu.ValidateProfile),
                    IconText = "user",
                    SelectionCommand = ValidateProfilesCommand
                },
                new MenuItemViewModel(AppServices)
                {
                    Text = Translate(TranslationKeys.MainMenu.Settings),
                    IconText = "cog",
                    SelectionCommand = SettingsCommand
                }
            };
		}

		public IMvxCommand ValidateProfilesCommand { get; }
        public IMvxCommand LogoutCommand { get; }
        public IMvxCommand SettingsCommand { get; }
        public IMvxCommand WalletCommand { get; }

        private void ValidateProfiles()
        {
			MenuManager.HideMenu();
            AppNavigationService.ShowValidateProfile();
        }

        private void Logout()
		{
            Alert(TranslationKeys.MainMenu.ConfirmLogout, async () =>
            {
                await MenuManager.HideMenu();
                await _logoutService.RegularLogout().ConfigureAwait(false);
            }, TranslationKeys.General.Yes, TranslationKeys.General.No);
		}

        private void Settings()
        {
            MenuManager.HideMenu();
            AppNavigationService.ShowSettings();
        }

        private void Wallet()
        {
            MenuManager.HideMenu();
            AppNavigationService.ShowWallet();
        }

        public ObservableCollection<MenuItemViewModel> Items { get; private set; }
    }
}
