using Coinstantine.Core.Services;
using Coinstantine.Core.ViewModels.Aidrops;
using Coinstantine.Core.ViewModels.Presale;
using Coinstantine.Core.ViewModels.ProfileValidation;
using Coinstantine.Core.ViewModels.Settings;
using Coinstantine.Core.ViewModels.Wallet;
using Coinstantine.Data;
using static Coinstantine.Core.ViewModels.Generic.DisplayInfoGenericViewModel;

namespace Coinstantine.Core.ViewModels.Generic
{
    public class DisplayInfoGenericViewModel : BaseViewModel<(object Param, Topic ThirdPartyItem)>
    {
        private readonly ValidateBitcoinTalkProfileViewModel _validateBitcoinTalkProfileViewModel;
        private readonly ValidateTwitterProfileViewModel _twitterProfileViewModel;
        private readonly ValidateTelegramProfileViewModel _telegramProfileViewModel;
        private readonly PresalePurchaseViewModel _presalePurchaseViewModel;
        private readonly AboutViewModel _aboutViewModel;
        private readonly AirdropViewModel _airdropViewModel;
        private readonly WalletViewModel _walletViewModel;

        public DisplayInfoGenericViewModel(IAppServices appServices,
                                           ValidateBitcoinTalkProfileViewModel bitcoinTalkProfileViewModel,
                                           ValidateTwitterProfileViewModel twitterProfileViewModel,
                                           ValidateTelegramProfileViewModel telegramProfileViewModel,
                                           PresalePurchaseViewModel presalePurchaseViewModel,  
                                           AboutViewModel aboutViewModel,  
                                           AirdropViewModel airdropViewModel,
                                           WalletViewModel walletViewModel) : base(appServices)
        {
            _validateBitcoinTalkProfileViewModel = bitcoinTalkProfileViewModel;
            _validateBitcoinTalkProfileViewModel.ParentViewModel = this;
            _twitterProfileViewModel = twitterProfileViewModel;
            _twitterProfileViewModel.ParentViewModel = this;
            _telegramProfileViewModel = telegramProfileViewModel;
            _presalePurchaseViewModel = presalePurchaseViewModel;
            _aboutViewModel = aboutViewModel;
            _airdropViewModel = airdropViewModel;
            _walletViewModel = walletViewModel;
            _telegramProfileViewModel.ParentViewModel = this;
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            ActiveViewModel?.ViewAppeared();
        }

        public override void ViewAppearing()
        {
            base.ViewAppearing();
            ActiveViewModel?.ViewAppearing();
        }

        public override void ViewCreated()
        {
            base.ViewCreated();
            ActiveViewModel?.ViewCreated();
        }

        public override void ViewDisappeared()
        {
            base.ViewDisappeared();
            ActiveViewModel?.ViewDisappeared();
        }

        public override void ViewDisappearing()
        {
            base.ViewDisappearing();
            ActiveViewModel?.ViewDisappearing();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            ActiveViewModel?.ViewDestroy(viewFinishing);
            base.ViewDestroy(viewFinishing);
        }

        public override void Prepare((object Param, Topic ThirdPartyItem) parameter)
        {
            var userProfile = parameter.Param as UserProfile;
            switch (parameter.ThirdPartyItem)
            {
                case Topic.BitcoinTalk:
                    _validateBitcoinTalkProfileViewModel.Prepare(userProfile.BitcoinTalkProfile);
                    ActiveViewModel = _validateBitcoinTalkProfileViewModel;
                    break;
                case Topic.Twitter:
                    _twitterProfileViewModel.Prepare(userProfile.TwitterProfile);
                    ActiveViewModel = _twitterProfileViewModel;
                    break;
                case Topic.Telegram:
                    _telegramProfileViewModel.Prepare(userProfile.TelegramProfile);
                    ActiveViewModel = _telegramProfileViewModel;
                    break;
                case Topic.PresalePurchase:
                    _presalePurchaseViewModel.Prepare(parameter.Param as BuyingReceipt);
                    ActiveViewModel = _presalePurchaseViewModel;
                    break;
                case Topic.Airdrop:
                    _airdropViewModel.Prepare(parameter.Param as AirdropDefinition);
                    ActiveViewModel = _airdropViewModel;
                    break;
                case Topic.About:
                    _aboutViewModel.Prepare(parameter.Param);
                    ActiveViewModel = _aboutViewModel;
                    break;
                case Topic.Wallet:
                    _walletViewModel.Prepare(parameter.Param);
                    ActiveViewModel = _walletViewModel;
                    break;
            }
            Title = ActiveViewModel.Title;
            TitleIcon = ActiveViewModel.TitleIcon;
            RaisePropertyChanged(nameof(Title));
        }

        public IGenericInfoViewModel ActiveViewModel { get; private set; }

        public enum Topic
        {
            Twitter,
            Telegram,
            BitcoinTalk,
            PresalePurchase,
            Airdrop,
            About,
            Wallet
        }
    }
}
