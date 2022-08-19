using System.Collections.Generic;
using System.Threading.Tasks;
using Coinstantine.Common;
using Coinstantine.Core.Services;
using Coinstantine.Core.ViewModels.Generic;
using Coinstantine.Core.ViewModels.ProfileValidation;
using Coinstantine.Data;
using Coinstantine.Domain.Airdrops;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Blockchain;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.ViewModels.Wallet
{
    public class WalletViewModel : GenericInfoViewModel<object>
    {
        private readonly IBlockchainService _blockchainService;
        private BlockchainInfo _blockchainInfo;
        public WalletViewModel(IAppServices appServices,
                               IProfileProvider profileProvider,
                               IBlockchainService blockchainService,
                               IUserService userService,
                               IGenericInfoItemViewModelConstructor itemInfoViewModelConstructor) : base(appServices, profileProvider, userService, itemInfoViewModelConstructor)
        {
            Title = TranslationKeys.MainMenu.Wallet;
            TitleIcon = "wallet";
            _blockchainService = blockchainService;
            RefreshText = Translate(TranslationKeys.Home.RefreshingMessage);
            HasRefreshingCapability = true;
        }

        public override async void ViewAppearing()
        {
            base.ViewAppearing();
            if (_blockchainInfo?.Coinstantine == 0 && _blockchainInfo?.Ether == 0)
            {
                await UpdateBalance().ConfigureAwait(false);
            }
            TrackPage("Wallet page");
        }

        public override async void Prepare(object parameter)
        {
            await Populate().ConfigureAwait(false);
        }

        protected async Task Populate()
        {
            var user = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            _blockchainInfo = user?.BlockchainInfo;
            string address = Translate(TranslationKeys.Wallet.NoAddress);
            var bctAddressSet = await _blockchainService.CanWithdrawBalance().ConfigureAwait(false);
            EnabledAction = bctAddressSet && _blockchainInfo?.Ether > 0;
            if (bctAddressSet)
            {
                address = user?.BitcoinTalkProfile?.Location;
            }

            GenericInfoItems = new MvxObservableCollection<GenericInfoItemViewModel>(_itemInfoViewModelConstructor.Construct(new List<ItemInfo>{
                { (TranslationKeys.General.Coinstantine, _blockchainInfo?.Coinstantine.ToString(), Display.New) },
                { (TranslationKeys.General.Ether, _blockchainInfo?.Ether.ToString(), Display.Grouped) },
                { (TranslationKeys.Wallet.WalletAddress, address, Display.New)}
            }));
        }

        protected override async Task Refresh()
        {
            IsRefreshing = true;
            await UpdateBalance().ConfigureAwait(false);
            IsRefreshing = false;
        }

        private async Task UpdateBalance()
        {
            await _blockchainService.UpdateBalance().ConfigureAwait(false);
            await Populate().ConfigureAwait(false);
            RaisePropertyChanged(nameof(GenericInfoItems));
        }

        public override string InfoTitle => Translate(TranslationKeys.Wallet.Balance);

        public override bool ShowRegularBehaviourText => true;
        public override bool ShowPrincipalButton => true;
        protected override string RegularBehaviourText => Translate(TranslationKeys.Wallet.WithdrawEther);
        public new string SecondaryButtonText => Translate(TranslationKeys.Wallet.Help);

        protected override Task SecondaryButtonAction()
        {
            Alert(Translate(TranslationKeys.Wallet.WhyNoAddress), () =>
            {
                DismissAlert();
                AppNavigationService.ShowAboutPage();
            },
                  TranslationKeys.Wallet.ReachTeam, TranslationKeys.General.Ok);

            return Task.FromResult(0);
        }

        protected override int GetRemainingTime()
        {
            return 0;
        }

        protected override async Task PrincipalButtonAction()
        {
            var user = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            var addressTo = user?.BitcoinTalkProfile?.Location;
            if (EnabledAction && addressTo != null && _blockchainInfo?.Ether > 0)
            {

                var message = string.Format(Translate(TranslationKeys.Wallet.WithdrawQuestion), addressTo);

                Alert(message, async () =>
                {
                    DismissAlert();
                    await WithdrawBalance(addressTo);
                });
            }
        }

        private async Task WithdrawBalance(string addressTo)
        {
            if (!AppServices.Connectivity.IsConnected)
            {
                ShowNoConnectionMessage();
                return;
            }
            Wait(TranslationKeys.Wallet.SendingTransaction);
            var receipt = await _blockchainService.WithdrawBalance(addressTo).ConfigureAwait(false);
            DismissWaitMessage();
            if (receipt.IsNullOrEmpty())
            {
                Alert(TranslationKeys.Wallet.AlertTransactionError);
                return;
            }
            Alert(TranslationKeys.Wallet.SuccessfullySent);
        }
    }
}
