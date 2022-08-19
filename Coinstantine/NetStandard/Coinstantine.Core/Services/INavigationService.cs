using Coinstantine.Data;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.Services
{
	public interface INavigationService
    {
        void ShowLandingPage();
        void ShowHomePage();
        void ShowWallet();
        void ShowValidateProfile();
        void ShowViewModel(MvxViewModel completeViewModel);
        void ShowSetPincode();
        void ShowResetPincode();
        void ShowCheckPincode();
        void ShowSettings();
        void ShowTelegramPage();
        void ShowBitcoinTalkPage();
        void ShowBitcoinTalkPage(UserProfile userProfile);
        void ShowBitcoinTalkPage(BitcoinTalkProfile bitcoinTalkProfile);
        void ShowTwitterPage();
        void ShowTwitterPage(UserProfile userProfile);
        void ShowChangeLanguage();
        void ShowTwitterPage(TwitterProfile twitterProfile);
        void ShowTelegramPage(UserProfile userProfile);
        void ShowTelegramPage(TelegramProfile telegramProfile);
        void ShowPurchasePage(BuyingReceipt buyingReceipt);
        void ShowAidropDetailPage(AirdropDefinition airdropDefinition);
        void ShowAboutPage();
        void ShowCreateAccount();
        void ShowLoginPage();
    }
}
