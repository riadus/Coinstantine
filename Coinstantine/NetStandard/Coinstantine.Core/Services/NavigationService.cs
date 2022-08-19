using Coinstantine.Common.Attributes;
using Coinstantine.Core.ViewModels;
using Coinstantine.Core.ViewModels.Account.AccountCreation;
using Coinstantine.Core.ViewModels.Account.Login;
using Coinstantine.Core.ViewModels.Generic;
using Coinstantine.Core.ViewModels.Home;
using Coinstantine.Core.ViewModels.ProfileValidation;
using Coinstantine.Core.ViewModels.Settings;
using Coinstantine.Data;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using static Coinstantine.Core.ViewModels.Generic.DisplayInfoGenericViewModel;

namespace Coinstantine.Core.Services
{
    [RegisterInterfaceAsLazySingleton]
    public class NavigationService : INavigationService
    {
        private readonly IMvxNavigationService _mvxNavigationService;
        public NavigationService(IMvxNavigationService mvxNavigationService)
        {
            _mvxNavigationService = mvxNavigationService;
        }

        public void ShowHomePage()
        {
            _mvxNavigationService.Navigate<HomeViewModel>();
        }

        public void ShowLandingPage()
        {
            _mvxNavigationService.Navigate<LandingPageViewModel>();
        }

        public void ShowSetPincode()
        {
            _mvxNavigationService.Navigate<SetPincodeViewModel, PincodeViewModel.PincodeType>(PincodeViewModel.PincodeType.SetPinCode);
        }

        public void ShowCheckPincode()
        {
            _mvxNavigationService.Navigate<SetPincodeViewModel, PincodeViewModel.PincodeType>(PincodeViewModel.PincodeType.CheckPinCode);
        }

        public void ShowResetPincode()
        {
            _mvxNavigationService.Navigate<SetPincodeViewModel, PincodeViewModel.PincodeType>(PincodeViewModel.PincodeType.ResetPinCode);
        }

        public void ShowValidateProfile()
        {
            _mvxNavigationService.Navigate<ValidateProfileViewModel>();
        }

        public void ShowViewModel(MvxViewModel viewModel)
        {
            _mvxNavigationService.Navigate(viewModel);
        }

        public void ShowSettings()
        {
            _mvxNavigationService.Navigate<SettingsViewModel>();
        }

        public void ShowTwitterPage()
        {
            _mvxNavigationService.Navigate<TwitterViewModel>();
        }

        public void ShowTelegramPage()
        {
            _mvxNavigationService.Navigate<TelegramViewModel>();
        }

        public void ShowBitcoinTalkPage()
        {
            _mvxNavigationService.Navigate<BitcoinTalkProfileViewModel>();
        }

        public void ShowBitcoinTalkPage(UserProfile userProfile)
        {
            ShowGenericPage(userProfile, Topic.BitcoinTalk);
        }

        public void ShowBitcoinTalkPage(BitcoinTalkProfile profile)
        {
            ShowGenericPage(new UserProfile { BitcoinTalkProfile = profile }, Topic.BitcoinTalk);
        }

        private void ShowGenericPage(object parameter, Topic thirdPartyItem)
        {
            _mvxNavigationService.Navigate<DisplayInfoGenericViewModel, (object Parameter, Topic ThirdPartyItem)>((parameter, thirdPartyItem));
        }

        public void ShowTwitterPage(UserProfile userProfile)
        {
            ShowGenericPage(userProfile, Topic.Twitter);
        }

        public void ShowTwitterPage(TwitterProfile twitterProfile)
        {
            ShowGenericPage(new UserProfile { TwitterProfile = twitterProfile }, Topic.Twitter);
        }

        public void ShowTelegramPage(UserProfile userProfile)
        {
            ShowGenericPage(userProfile, Topic.Telegram);
        }

        public void ShowTelegramPage(TelegramProfile telegramProfile)
        {
            ShowGenericPage(new UserProfile { TelegramProfile = telegramProfile }, Topic.Telegram);
        }

        public void ShowChangeLanguage()
        {
            _mvxNavigationService.Navigate<ChangeLanguageViewModel>();
        }

        public void ShowPurchasePage(BuyingReceipt buyingReceipt)
        {
            ShowGenericPage(buyingReceipt, Topic.PresalePurchase);
        }

        public void ShowAidropDetailPage(AirdropDefinition airdropDefinition)
        {
            ShowGenericPage(airdropDefinition, Topic.Airdrop);
        }

        public void ShowAboutPage()
        {
            ShowGenericPage(null, Topic.About);
        }

        public void ShowWallet()
        {
            ShowGenericPage(null, Topic.Wallet);
        }

        public void ShowCreateAccount()
        {
            _mvxNavigationService.Navigate<CreateAccountViewModel>();
        }

        public void ShowLoginPage()
        {
            _mvxNavigationService.Navigate<LoginViewModel>();
        }
    }
}
